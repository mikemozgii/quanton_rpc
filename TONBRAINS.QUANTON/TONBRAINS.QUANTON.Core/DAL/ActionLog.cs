using System;
using System.ComponentModel.DataAnnotations;

namespace TONBRAINS.QUANTON.Core.DAL
{
    public class ActionLog
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string ActionId { get; set; }
        public string UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreationDate { get; set; }
        [Display(Name = "platform")] public string Platform { get; set; }
        [Display(Name = "latitude")] public string Latitude { get; set; }
        [Display(Name = "longitude")] public string Longitude { get; set; }
        [Display(Name = "timezone")] public string TimeZone { get; set; }
        [Display(Name = "timestamp")] public string TimeStamp { get; set; }
        [Display(Name = "deviceid")] public string DeviceId { get; set; }
        [Display(Name = "model")] public string Model { get; set; }
        [Display(Name = "isphysicaldevice")] public string IsPhysicalDevice { get; set; }
        [Display(Name = "release")] public string Release { get; set; }
        [Display(Name = "securitypatch")] public string SecurityPatch { get; set; }
        [Display(Name = "sdk")] public string Sdk { get; set; }
        [Display(Name = "incremental")] public string Incremental { get; set; }
        [Display(Name = "codename")] public string Codename { get; set; }
        [Display(Name = "baseos")] public string BaseOS { get; set; }
        [Display(Name = "board")] public string Board { get; set; }
        [Display(Name = "bootloader")] public string Bootloader { get; set; }
        [Display(Name = "brand")] public string Brand { get; set; }
        [Display(Name = "device")] public string Device { get; set; }
        [Display(Name = "display")] public string Display { get; set; }
        [Display(Name = "fingerprint")] public string Fingerprint { get; set; }
        [Display(Name = "hardware")] public string Hardware { get; set; }
        [Display(Name = "buildhost")] public string BuildHost { get; set; }
        [Display(Name = "buildid")] public string BuildId { get; set; }
        [Display(Name = "manufacturer")] public string Manufacturer { get; set; }
        [Display(Name = "product")] public string Product { get; set; }
        [Display(Name = "tags")] public string Tags { get; set; }
        [Display(Name = "type")] public string Type { get; set; }
        [Display(Name = "name")] public string Name { get; set; }
        [Display(Name = "systemname")] public string SystemName { get; set; }
        [Display(Name = "systemversion")] public string SystemVersion { get; set; }
        [Display(Name = "localizedmodel")] public string LocalizedModel { get; set; }
        [Display(Name = "sysname")] public string Sysname { get; set; }
        [Display(Name = "nodename")] public string Nodename { get; set; }
        [Display(Name = "version")] public string Version { get; set; }
        [Display(Name = "machine")] public string Machine { get; set; }
    }
}
