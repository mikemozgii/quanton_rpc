using Grpc.Core;
using Grpc.Core.Interceptors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TONBRAINS.QUANTON.Grpc
{
    public class SessionInterceptor : Interceptor
    {

        private static HashSet<string> m_AuthentificationExceptions = new HashSet<string> {
            "/ton_mobile.TonMobile/SignIn",
            "/ton_mobile.TonMobile/AppVersion",
            "/ton_mobile.TonMobile/PublicSecretKeySignIn",
            "/ton_mobile.TonMobile/MnemonicPhraseSignIn",
        };

        private string GetValueFromMetadata(ServerCallContext context, string name)
        {
            var metadataEntry = context.RequestHeaders.FirstOrDefault(m => string.Equals(m.Key, name, StringComparison.Ordinal));
            if (metadataEntry == null || metadataEntry.Value == null) return null;

            return metadataEntry.Value;
        }

        private bool IsAuthentificated(ServerCallContext context)
        {
            if (m_AuthentificationExceptions.Contains(context.Method)) return true;

            var token = GetValueFromMetadata(context, "token");
            if (string.IsNullOrEmpty(token) || !JwtService.ValidateToken(token)) return false;

            var tokenData = JwtService.ParseToken(token);

            context.UserState.Add("session", JsonConvert.SerializeObject(tokenData));

            return true;
        }

        private void AuthentificationHandler(ServerCallContext context)
        {
            var isContinue = IsAuthentificated(context);
            if (!isContinue) throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"), "Authentification failed");
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            AuthentificationHandler(context);

            return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            AuthentificationHandler(context);

            return base.UnaryServerHandler(request, context, continuation);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            AuthentificationHandler(context);

            return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            AuthentificationHandler(context);
            return base.ClientStreamingServerHandler(requestStream, context, continuation);
        }

    }
}
