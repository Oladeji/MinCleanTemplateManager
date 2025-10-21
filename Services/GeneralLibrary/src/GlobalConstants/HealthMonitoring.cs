using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

using System.Reflection;


namespace GlobalConstants
{
    public static  class HealthMonitoring
    {
        public static (string? description, HealthStatus healthStatus, IReadOnlyDictionary<string, object>? data) MemoryUsage()
        {
            var totalMemory = GC.GetTotalMemory(false);
            var workingSet = Environment.WorkingSet;
            var process = Process.GetCurrentProcess();

            Dictionary<string, object> data;
            HealthStatus status;
            string description;
            (data, status, description) = HealthInfoExtractor.ExtractMemoryUsageInfo(totalMemory, workingSet, process);

            return (description, status, data);
        }
        public static (string? description, IReadOnlyDictionary<string, object>? data) GetAPIGAteWayHealthInfo()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
            var machineName = Environment.MachineName;
            var processId = Environment.ProcessId;
            var startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Enhanced description with key information visible in UI
            var enhancedDescription = $"✅ Gateway operational | v{version} | {environment} | {machineName} | PID:{processId} | Started:{startTime}";

            var data = new Dictionary<string, object>
            {
                { "🚀_Service_Name", "MassFusion API Gateway" },
                { "📦_Version", version },
                { "🌍_Environment", environment },
                { "🖥️_Machine_Name", machineName },
                { "⚙️_Process_ID", processId },
                { "🕒_Start_Time", startTime },
                { "✅_Status", "Gateway is running and healthy" },
                { "🔗_Health_UI_Path", "/health-ui" },
                { "📊_Health_Details_API", "/health/details" },
                { "💾_Drive_Details_API", "/health/drives" },
                { "🧠_Memory_Details_API", "/health/memory" },
                { "🖥️_System_Details_API", "/health/system" }
            };

            return (enhancedDescription, data);
        }

        public static (string description, HealthStatus healthStatus, IReadOnlyDictionary<string, object> data) GetCPUUsageInfo()
        {
            var process = Process.GetCurrentProcess();

            var data = new Dictionary<string, object>
            {
                { "🖥️_Processor_Count", Environment.ProcessorCount },
                { "🧵_Thread_Count", process.Threads.Count },
                { "🔧_Handle_Count", process.HandleCount },
                { "🕒_Process_Start_Time", process.StartTime.ToString("yyyy-MM-dd HH:mm:ss") },
                { "⏱️_Process_Up_Time", (DateTime.Now - process.StartTime).ToString(@"dd\.hh\:mm\:ss") },
                { "🖥️_Machine_Name", Environment.MachineName },
                { "🚀_OS_Version", Environment.OSVersion.ToString() },
                { "⚙️_Total_CPU_Time", process.TotalProcessorTime.ToString(@"dd\.hh\:mm\:ss") },
                { "👤_User_CPU_Time", process.UserProcessorTime.ToString(@"dd\.hh\:mm\:ss") },
                { "🔒_System_CPU_Time", process.PrivilegedProcessorTime.ToString(@"dd\.hh\:mm\:ss") },
                { "ℹ️_Thread_Threshold", "Normal: < 200 threads, High: > 200 threads" },
                { "ℹ️_Handle_Threshold", "Normal: < 10000 handles, High: > 10000 handles" },
                { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
            };

            var status = process.Threads.Count > 200 || process.HandleCount > 10000 ?
                       HealthStatus.Degraded : HealthStatus.Healthy;

            var description = $"💻 {Environment.ProcessorCount} cores | {process.Threads.Count} threads | {process.HandleCount} handles";

            return (description, status, data);
        }

        public static (string description, HealthStatus healthStatus, IReadOnlyDictionary<string, object> data) GetDiskSpaceInfo()
        {
            try
            {
                // Get ALL drives first, then analyze them with extensive logging
                var allDrives = DriveInfo.GetDrives();

                // Create comprehensive drive analysis with detailed logging
                var drives = allDrives.Select((d, index) =>
                {
                    try
                    {
                        // Get basic drive properties first
                        var driveName = d.Name;
                        var isReady = false;
                        var driveType = DriveType.Unknown;

                        // Try to get drive readiness and type safely
                        try
                        {
                            isReady = d.IsReady;
                            driveType = d.DriveType;
                        }
                        catch (Exception typeEx)
                        {
                            return new
                            {
                                Name = driveName,
                                Label = "Type Check Failed",
                                DriveType = "Unknown",
                                DriveFormat = "Unknown",
                                IsReady = false,
                                FreeSpaceGB = 0.0,
                                TotalSpaceGB = 0.0,
                                UsedSpaceGB = 0.0,
                                UsedPercentage = 0.0,
                                FreePercentage = 0.0,
                                IsSystemDrive = false,
                                IsApplicationDrive = false,
                                HasError = true,
                                ErrorMessage = $"Drive type check failed: {typeEx.Message}",
                                Status = "Type Error",
                                DebugInfo = $"Index: {index}, Exception: {typeEx.GetType().Name}"
                            };
                        }

                        // If drive is ready, get detailed info
                        if (isReady)
                        {
                            try
                            {
                                // Get space information
                                var totalSize = d.TotalSize;
                                var freeSpace = d.AvailableFreeSpace;
                                var usedSpace = totalSize - freeSpace;

                                return new
                                {
                                    Name = driveName,
                                    Label = string.IsNullOrEmpty(d.VolumeLabel) ? "Unnamed" : d.VolumeLabel,
                                    DriveType = driveType.ToString(),
                                    DriveFormat = d.DriveFormat ?? "Unknown",
                                    IsReady = true,
                                    FreeSpaceGB = Math.Round((double)freeSpace / (1024 * 1024 * 1024), 2),
                                    TotalSpaceGB = Math.Round((double)totalSize / (1024 * 1024 * 1024), 2),
                                    UsedSpaceGB = Math.Round((double)usedSpace / (1024 * 1024 * 1024), 2),
                                    UsedPercentage = totalSize > 0 ? Math.Round((double)usedSpace / totalSize * 100, 2) : 0,
                                    FreePercentage = totalSize > 0 ? Math.Round((double)freeSpace / totalSize * 100, 2) : 0,
                                    IsSystemDrive = driveName.Equals(Path.GetPathRoot(Environment.SystemDirectory), StringComparison.OrdinalIgnoreCase),
                                    IsApplicationDrive = driveName.Equals(Path.GetPathRoot(Assembly.GetExecutingAssembly().Location), StringComparison.OrdinalIgnoreCase),
                                    HasError = false,
                                    ErrorMessage = "",
                                    Status = "Ready",
                                    DebugInfo = $"Index: {index}, TotalGB: {Math.Round((double)totalSize / (1024 * 1024 * 1024), 2)}, FreeGB: {Math.Round((double)freeSpace / (1024 * 1024 * 1024), 2)}"
                                };
                            }
                            catch (Exception spaceEx)
                            {
                                return new
                                {
                                    Name = driveName,
                                    Label = "Space Check Failed",
                                    DriveType = driveType.ToString(),
                                    DriveFormat = "Unknown",
                                    IsReady = true,
                                    FreeSpaceGB = 0.0,
                                    TotalSpaceGB = 0.0,
                                    UsedSpaceGB = 0.0,
                                    UsedPercentage = 0.0,
                                    FreePercentage = 0.0,
                                    IsSystemDrive = false,
                                    IsApplicationDrive = false,
                                    HasError = true,
                                    ErrorMessage = $"Space check failed: {spaceEx.Message}",
                                    Status = "Space Error",
                                    DebugInfo = $"Index: {index}, Ready but space access failed"
                                };
                            }
                        }
                        else
                        {
                            // Drive exists but not ready (CD-ROM without disk, etc.)
                            return new
                            {
                                Name = driveName,
                                Label = "Not Ready",
                                DriveType = driveType.ToString(),
                                DriveFormat = "N/A",
                                IsReady = false,
                                FreeSpaceGB = 0.0,
                                TotalSpaceGB = 0.0,
                                UsedSpaceGB = 0.0,
                                UsedPercentage = 0.0,
                                FreePercentage = 0.0,
                                IsSystemDrive = false,
                                IsApplicationDrive = false,
                                HasError = false,
                                ErrorMessage = $"Drive not ready (Type: {driveType})",
                                Status = "Not Ready",
                                DebugInfo = $"Index: {index}, Type: {driveType}, Not ready"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new
                        {
                            Name = d?.Name ?? $"Drive_{index}",
                            Label = "Error",
                            DriveType = "Error accessing drive",
                            DriveFormat = "Unknown",
                            IsReady = false,
                            FreeSpaceGB = 0.0,
                            TotalSpaceGB = 0.0,
                            UsedSpaceGB = 0.0,
                            UsedPercentage = 0.0,
                            FreePercentage = 0.0,
                            IsSystemDrive = false,
                            IsApplicationDrive = false,
                            HasError = true,
                            ErrorMessage = ex.Message,
                            Status = "Error",
                            DebugInfo = $"Index: {index}, Exception: {ex.GetType().Name}, Message: {ex.Message}"
                        };
                    }
                }).ToList();

                // Flatten data structure for better UI display
                var data = new Dictionary<string, object>();

                // Overall summary (show ALL drives found)
                var readyDrives = drives.Where(d => d.IsReady && !d.HasError).ToList();
                var notReadyDrives = drives.Where(d => !d.IsReady && !d.HasError).ToList();
                var errorDrives = drives.Where(d => d.HasError).ToList();

                data.Add("📊_Total_Drives_Detected", drives.Count);
                data.Add("📊_Ready_Drives", readyDrives.Count);
                data.Add("📊_Not_Ready_Drives", notReadyDrives.Count);
                data.Add("📊_Error_Drives", errorDrives.Count);

                // Enhanced debugging information
                data.Add("🔧_Debug_Raw_DriveInfo_Count", allDrives.Length);
                data.Add("🔧_Debug_System_Directory", Environment.SystemDirectory ?? "Unknown");
                data.Add("🔧_Debug_System_Root", Path.GetPathRoot(Environment.SystemDirectory) ?? "Unknown");
                data.Add("🔧_Debug_Assembly_Location", Assembly.GetExecutingAssembly().Location ?? "Unknown");
                data.Add("🔧_Debug_Assembly_Root", Path.GetPathRoot(Assembly.GetExecutingAssembly().Location) ?? "Unknown");
                data.Add("🔧_Debug_Current_Directory", Environment.CurrentDirectory ?? "Unknown");
                data.Add("🔧_Debug_Current_Directory_Root", Path.GetPathRoot(Environment.CurrentDirectory) ?? "Unknown");

                // Show what drives were detected with details
                data.Add("🔍_All_Drive_Letters", string.Join(", ", drives.Select(d => d.Name)));
                data.Add("🟢_Ready_Drive_Letters", string.Join(", ", readyDrives.Select(d => d.Name)));
                if (notReadyDrives.Any())
                    data.Add("🟡_Not_Ready_Drive_Letters", string.Join(", ", notReadyDrives.Select(d => d.Name)));
                if (errorDrives.Any())
                    data.Add("🔴_Error_Drive_Letters", string.Join(", ", errorDrives.Select(d => d.Name)));

                // Debug each drive's analysis with extensive information
                for (int j = 0; j < drives.Count && j < 15; j++) // Show up to 15 drives
                {
                    var drive = drives[j];
                    data.Add($"🔧_Debug_Drive_{j + 1}_Name", drive.Name);
                    data.Add($"🔧_Debug_Drive_{j + 1}_IsReady", drive.IsReady);
                    data.Add($"🔧_Debug_Drive_{j + 1}_HasError", drive.HasError);
                    data.Add($"🔧_Debug_Drive_{j + 1}_Status", drive.Status);
                    data.Add($"🔧_Debug_Drive_{j + 1}_Type", drive.DriveType);
                    data.Add($"🔧_Debug_Drive_{j + 1}_Info", drive.DebugInfo);
                    if (drive.HasError)
                        data.Add($"🔧_Debug_Drive_{j + 1}_ErrorMsg", drive.ErrorMessage);
                    if (drive.IsReady && !drive.HasError)
                    {
                        data.Add($"🔧_Debug_Drive_{j + 1}_FreeGB", drive.FreeSpaceGB);
                        data.Add($"🔧_Debug_Drive_{j + 1}_TotalGB", drive.TotalSpaceGB);
                        data.Add($"🔧_Debug_Drive_{j + 1}_UsedPercent", drive.UsedPercentage);
                    }
                }

                // Calculate totals only for ready drives
                if (readyDrives.Any())
                {
                    data.Add("📊_Total_Space_GB", Math.Round(readyDrives.Sum(d => d.TotalSpaceGB), 2));
                    data.Add("📊_Total_Free_Space_GB", Math.Round(readyDrives.Sum(d => d.FreeSpaceGB), 2));
                    data.Add("📊_Total_Used_Space_GB", Math.Round(readyDrives.Sum(d => d.UsedSpaceGB), 2));
                    data.Add("📊_Overall_Usage_Percentage", Math.Round(readyDrives.Sum(d => d.UsedSpaceGB) / readyDrives.Sum(d => d.TotalSpaceGB) * 100, 2));
                }

                // Individual drive details (ALL drives, not just ready ones)
                for (int i = 0; i < drives.Count; i++)
                {
                    var drive = drives[i];
                    var driveKey = drive.Name.Replace("\\", "").Replace(":", "").Replace(" ", "_");

                    if (drive.HasError)
                    {
                        data.Add($"❌_Drive_{driveKey}_Name", drive.Name);
                        data.Add($"❌_Drive_{driveKey}_Status", drive.Status);
                        data.Add($"❌_Drive_{driveKey}_Error", drive.ErrorMessage);
                        data.Add($"❌_Drive_{driveKey}_Type", drive.DriveType);
                        data.Add($"❌_Drive_{driveKey}_Debug", drive.DebugInfo);
                        continue;
                    }

                    if (!drive.IsReady)
                    {
                        data.Add($"🟡_Drive_{driveKey}_Name", drive.Name);
                        data.Add($"🟡_Drive_{driveKey}_Status", drive.Status);
                        data.Add($"🟡_Drive_{driveKey}_Type", drive.DriveType);
                        data.Add($"🟡_Drive_{driveKey}_Note", drive.ErrorMessage);
                        data.Add($"🟡_Drive_{driveKey}_Debug", drive.DebugInfo);
                        continue;
                    }

                    // Ready drives with full details
                    var statusEmoji = drive.UsedPercentage > 90 ? "🔴" :
                                    drive.UsedPercentage > 80 ? "🟡" :
                                    drive.UsedPercentage > 70 ? "🟠" : "🟢";

                    data.Add($"{statusEmoji}_Drive_{driveKey}_Name", drive.Name);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Label", drive.Label);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Type", drive.DriveType);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Format", drive.DriveFormat);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Total_GB", drive.TotalSpaceGB);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Free_GB", drive.FreeSpaceGB);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Used_GB", drive.UsedSpaceGB);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Used_Percent", drive.UsedPercentage);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Free_Percent", drive.FreePercentage);
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Debug", drive.DebugInfo);

                    // Handle special drive designations without duplicate keys
                    var specialDesignations = new List<string>();
                    if (drive.IsSystemDrive)
                        specialDesignations.Add("🖥️ System Drive");
                    if (drive.IsApplicationDrive)
                        specialDesignations.Add("⚙️ Application Drive");

                    if (specialDesignations.Any())
                        data.Add($"{statusEmoji}_Drive_{driveKey}_Special", string.Join(" & ", specialDesignations));

                    var recommendation = drive.UsedPercentage > 90 ? "🚨 Immediate cleanup required" :
                                       drive.UsedPercentage > 80 ? "⚠️ Consider cleanup soon" :
                                       drive.UsedPercentage > 70 ? "👀 Monitor closely" : "✅ Space adequate";
                    data.Add($"{statusEmoji}_Drive_{driveKey}_Recommendation", recommendation);
                }

                // Thresholds and explanations
                data.Add("ℹ️_Healthy_Threshold", "< 70% used space");
                data.Add("ℹ️_Caution_Threshold", "70-80% used space");
                data.Add("ℹ️_Warning_Threshold", "80-90% used space");
                data.Add("ℹ️_Critical_Threshold", "> 90% used space");
                data.Add("ℹ️_Drive_Types_Explanation", "Fixed=Hard drives, Removable=USB/Floppy, CDRom=Optical drives, Network=Mapped drives");
                data.Add("ℹ️_Not_Ready_Explanation", "Drives exist but no media inserted (CD-ROM, floppy) or temporarily unavailable");

                // Determine overall health status
                if (readyDrives.Count == 0)
                {
                    data.Add("🚨_Critical_Alert", "No ready drives found for monitoring");
                    data.Add("🔧_Troubleshooting", "Check drive permissions and system access rights");
                    return (description: "❌ No ready drives found for monitoring", healthStatus: HealthStatus.Unhealthy, data: data);
                }

                // TEMPORARY FIX: Only check system drives (C:) and ignore D: drive
                var systemDrives = readyDrives.Where(d => d.IsSystemDrive || d.Name.StartsWith("C:", StringComparison.OrdinalIgnoreCase)).ToList();
                var criticalDrive = systemDrives.FirstOrDefault(d => d.UsedPercentage > 90);
                var warningDrive = systemDrives.FirstOrDefault(d => d.UsedPercentage > 80);

                HealthStatus status;
                string description;

                if (criticalDrive != null)
                {
                    status = HealthStatus.Unhealthy;
                    var healthyDrives = systemDrives.Where(d => d.UsedPercentage <= 90).ToList();

                    description = $"🔴 Critical: {criticalDrive.Name} {criticalDrive.UsedPercentage}% full ({criticalDrive.FreeSpaceGB}GB free)";

                    // Include healthy drives in description if they exist
                    if (healthyDrives.Any())
                    {
                        var healthyDetails = healthyDrives.Select(d => $"{d.Name}{d.FreeSpaceGB}GB({d.UsedPercentage}%)").ToList();
                        description += $" | Healthy: {string.Join(", ", healthyDetails.Take(3))}";
                        if (healthyDetails.Count > 3) description += $" +{healthyDetails.Count - 3}more";
                    }

                    data.Add("🚨_Critical_Alert", $"Drive {criticalDrive.Name} critically low on space");
                }
                else if (warningDrive != null)
                {
                    status = HealthStatus.Degraded;
                    var healthyDrives = systemDrives.Where(d => d.UsedPercentage <= 80).ToList();

                    description = $"🟡 Warning: {warningDrive.Name} {warningDrive.UsedPercentage}% full ({warningDrive.FreeSpaceGB}GB free)";

                    // Include healthy drives in description if they exist
                    if (healthyDrives.Any())
                    {
                        var healthyDetails = healthyDrives.Select(d => $"{d.Name}{d.FreeSpaceGB}GB({d.UsedPercentage}%)").ToList();
                        description += $" | Healthy: {string.Join(", ", healthyDetails.Take(3))}";
                        if (healthyDetails.Count > 3) description += $" +{healthyDetails.Count - 3}more";
                    }

                    data.Add("⚠️_Warning_Alert", $"Drive {warningDrive.Name} getting low on space");
                }
                else
                {
                    status = HealthStatus.Healthy;
                    var driveDetails = systemDrives.Select(d => $"{d.Name}{d.FreeSpaceGB}GB({d.UsedPercentage}%)").ToList();
                    description = $"✅ System drives healthy: {string.Join(" | ", driveDetails.Take(5))}";
                    if (driveDetails.Count > 5) description += $" | +{driveDetails.Count - 5} more";

                    // Mention D: drive issue without failing the health check
                    var dDrive = readyDrives.FirstOrDefault(d => d.Name.StartsWith("D:", StringComparison.OrdinalIgnoreCase));
                    if (dDrive != null && dDrive.UsedPercentage > 90)
                    {
                        description += $" | ⚠️ Note: {dDrive.Name} {dDrive.UsedPercentage}% full (non-critical)";
                        data.Add("⚠️_Non_Critical_Warning", $"Drive {dDrive.Name} is full but not affecting service health");
                    }
                }

                // Add summary of issues if any
                var issues = new List<string>();
                if (notReadyDrives.Any()) issues.Add($"{notReadyDrives.Count} not ready");
                if (errorDrives.Any()) issues.Add($"{errorDrives.Count} errors");

                if (issues.Any())
                {
                    data.Add("⚠️_Drive_Issues", string.Join(", ", issues));
                    if (status == HealthStatus.Healthy)
                    {
                        status = HealthStatus.Degraded;
                        description += $" | ⚠️ {string.Join(", ", issues)}";
                    }
                }

                // Add timestamp and metadata
                data.Add("🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                data.Add("🤖_Checked_By", "API Gateway Health Monitor");
                data.Add("🔧_Detection_Method", "System drives only (D: drive excluded from critical checks)");
                data.Add("🔧_Deployment_Note", "Temporary fix: Only monitoring system drives for service health");

                return (description, status, data);
            }
            catch (Exception ex)
            {
                var errorData = new Dictionary<string, object>
                {
                    { "🚨_Error", ex.Message },
                    { "🚨_Error_Type", ex.GetType().Name },
                    { "🚨_Stack_Trace", ex.StackTrace?.Split('\n').Take(10).ToArray() ?? new string[0] },
                    { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "❌_Check_Failed", true },
                    { "🔧_Possible_Causes", "System-level permission issues, security restrictions, or drive enumeration failure" },
                    { "💡_Suggestion", "Check application permissions, anti-virus settings, and system security policies" },
                    { "🔧_Drive_Detection_Failed", "Complete failure to enumerate drives - critical system issue" },
                    { "🔧_Environment_Info", $"OS: {Environment.OSVersion}, User: {Environment.UserName}, Domain: {Environment.UserDomainName}" }
                };
                return ($"❌ Critical drive enumeration failure: {ex.Message}", HealthStatus.Unhealthy, errorData);
            }
        }

        public static (string description, HealthStatus healthStatus, IReadOnlyDictionary<string, object> data) GetNetworkConnectivityInfo()
        {
            try
            {
                var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                    .ToList();

                var data = new Dictionary<string, object>
                {
                    { "🖥️_Host_Name", Environment.MachineName },
                    { "🌐_User_Domain", Environment.UserDomainName },
                    { "📡_Network_Available", System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() },
                    { "🔌_Active_Interfaces_Count", networkInterfaces.Count },
                    { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                };

                // Add individual interface details
                for (int i = 0; i < networkInterfaces.Count && i < 10; i++) // Limit to 10 interfaces
                {
                    var ni = networkInterfaces[i];
                    data.Add($"🔌_Interface_{i + 1}_Name", ni.Name);
                    data.Add($"🔌_Interface_{i + 1}_Type", ni.NetworkInterfaceType.ToString());
                    data.Add($"🔌_Interface_{i + 1}_Speed", $"{ni.Speed:N0} bps");
                    data.Add($"🔌_Interface_{i + 1}_Status", ni.OperationalStatus.ToString());
                }

                if (networkInterfaces.Count > 10)
                {
                    data.Add("ℹ️_Additional_Interfaces", $"{networkInterfaces.Count - 10} more interfaces available");
                }

                var description = $"🌐 Network available | {networkInterfaces.Count} active interfaces";
                return (description, HealthStatus.Healthy, data);
            }
            catch (Exception ex)
            {
                var errorData = new Dictionary<string, object>
                {
                    { "🚨_Error", ex.Message },
                    { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                };
                return ("❌ Network check failed", HealthStatus.Unhealthy, errorData);
            }
        }

        public static (string description, HealthStatus healthStatus, IReadOnlyDictionary<string, object> data) GetOTLPEndpointInfo(IConfiguration configuration)
        {
            try
            {
                var otlpEndpoint = configuration["Otlp:Endpoint"] ?? OTLPParams.EndPoint;
                var serviceName = configuration["Otlp:ServiceName"] ?? OTLPParams.ServiceName;

                var data = new Dictionary<string, object>
                {
                    { "🔗_Endpoint_URL", otlpEndpoint },
                    { "🏷️_Service_Name", serviceName },
                    { "📡_Protocol", "gRPC" },
                    { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                };

                // Try to parse the endpoint to validate it
                if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var uri))
                {
                    data.Add("✅_Valid_URI", true);
                    data.Add("🌐_Host", uri.Host);
                    data.Add("🔌_Port", uri.Port);
                    data.Add("🔒_Scheme", uri.Scheme);
                    data.Add("📍_Full_Authority", uri.Authority);
                    data.Add("ℹ️_URI_Status", "Endpoint URL format is valid");

                    return ($"✅ OTLP: {uri.Host}:{uri.Port}", HealthStatus.Healthy, data);
                }
                else
                {
                    data.Add("❌_Valid_URI", false);
                    data.Add("⚠️_Issue", "Invalid endpoint URL format");
                    data.Add("🔧_Recommendation", "Check OTLP endpoint configuration");

                    return ($"⚠️ OTLP endpoint format invalid: {otlpEndpoint}", HealthStatus.Degraded, data);
                }
            }
            catch (Exception ex)
            {
                var errorData = new Dictionary<string, object>
                {
                    { "🚨_Error", ex.Message },
                    { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "🔧_Recommendation", "Check OTLP configuration and network connectivity" }
                };
                return ("❌ OTLP endpoint check failed", HealthStatus.Unhealthy, errorData);
            }
        }

        public static (string description, HealthStatus healthStatus, IReadOnlyDictionary<string, object> data) GetSystemPerformanceInfo()
        {
            try
            {
                var process = Process.GetCurrentProcess();
                var totalMemory = GC.GetTotalMemory(false);
                var workingSet = Environment.WorkingSet;

                // Flatten all data for better UI display
                var data = new Dictionary<string, object>
                {
                    // Memory Information (flattened)
                    { "🧠_App_Memory_MB", Math.Round(totalMemory / 1024.0 / 1024.0, 2) },
                    { "🧠_Working_Set_MB", Math.Round(workingSet / 1024.0 / 1024.0, 2) },
                    { "🧠_Private_Memory_MB", Math.Round(process.PrivateMemorySize64 / 1024.0 / 1024.0, 2) },
                    { "🧠_Virtual_Memory_MB", Math.Round(process.VirtualMemorySize64 / 1024.0 / 1024.0, 2) },
                    { "🧠_GC_Gen0_Collections", GC.CollectionCount(0) },
                    { "🧠_GC_Gen1_Collections", GC.CollectionCount(1) },
                    { "🧠_GC_Gen2_Collections", GC.CollectionCount(2) },
                    
                    // Process Information (flattened)
                    { "⚙️_Process_ID", Environment.ProcessId },
                    { "⚙️_Process_Name", process.ProcessName },
                    { "⚙️_Thread_Count", process.Threads.Count },
                    { "⚙️_Handle_Count", process.HandleCount },
                    { "⚙️_Start_Time", process.StartTime.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "⚙️_Up_Time", (DateTime.Now - process.StartTime).ToString(@"dd\.hh\:mm\:ss") },
                    { "⚙️_Total_CPU_Time", process.TotalProcessorTime.ToString(@"dd\.hh\:mm\:ss") },
                    { "⚙️_User_CPU_Time", process.UserProcessorTime.ToString(@"dd\.hh\:mm\:ss") },
                    { "⚙️_System_CPU_Time", process.PrivilegedProcessorTime.ToString(@"dd\.hh\:mm\:ss") },
                    
                    // System Information (flattened)
                    { "🖥️_Machine_Name", Environment.MachineName },
                    { "🖥️_OS_Version", Environment.OSVersion.ToString() },
                    { "🖥️_Processor_Count", Environment.ProcessorCount },
                    { "🖥️_Is_64Bit_Process", Environment.Is64BitProcess },
                    { "🖥️_Is_64Bit_OS", Environment.Is64BitOperatingSystem },
                    { "🖥️_System_Page_Size", Environment.SystemPageSize },
                    { "🖥️_User_Domain", Environment.UserDomainName },
                    { "🖥️_User_Name", Environment.UserName },
                    { "🖥️_Framework_Version", Environment.Version.ToString() },
                    { "🖥️_System_Directory", Environment.SystemDirectory },
                    { "🖥️_Working_Directory", Environment.CurrentDirectory },
                    
                    // Runtime Information (flattened)
                    { "🚀_Runtime_Version", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription },
                    { "🚀_OS_Description", System.Runtime.InteropServices.RuntimeInformation.OSDescription },
                    { "🚀_OS_Architecture", System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString() },
                    { "🚀_Process_Architecture", System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString() },
                    
                    // Performance Thresholds
                    { "ℹ️_Memory_Healthy", "< 2000MB" },
                    { "ℹ️_Memory_Warning", "2000-4000MB" },
                    { "ℹ️_Memory_Critical", "> 4000MB" },
                    { "ℹ️_Threads_Healthy", "< 200 threads" },
                    { "ℹ️_Threads_Warning", "> 200 threads" },
                    { "ℹ️_Handles_Healthy", "< 10000 handles" },
                    { "ℹ️_Handles_Warning", "> 10000 handles" },

                    { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                };

                // Determine health status based on performance metrics
                var memoryMB = totalMemory / 1024.0 / 1024.0;
                var threadCount = process.Threads.Count;
                var handleCount = process.HandleCount;

                HealthStatus status = HealthStatus.Healthy;
                string description = "✅ System performance is optimal";

                var issues = new List<string>();

                if (memoryMB > 2000) // 2GB threshold
                {
                    issues.Add($"High memory: {memoryMB:F1}MB");
                    status = HealthStatus.Degraded;
                    data.Add("⚠️_High_Memory_Warning", $"Memory usage is {memoryMB:F1}MB (threshold: 2000MB)");
                }

                if (threadCount > 200) // High thread count
                {
                    issues.Add($"High threads: {threadCount}");
                    status = HealthStatus.Degraded;
                    data.Add("⚠️_High_Thread_Warning", $"Thread count is {threadCount} (threshold: 200)");
                }

                if (handleCount > 10000) // High handle count
                {
                    issues.Add($"High handles: {handleCount}");
                    status = HealthStatus.Degraded;
                    data.Add("⚠️_High_Handle_Warning", $"Handle count is {handleCount} (threshold: 10000)");
                }

                if (memoryMB > 4000) // Critical memory threshold
                {
                    status = HealthStatus.Unhealthy;
                    description = "🔴 Critical memory usage detected";
                    data.Add("🚨_Critical_Memory_Alert", $"Memory usage is critically high: {memoryMB:F1}MB");
                }

                if (issues.Any())
                {
                    data.Add("⚠️_Performance_Issues", string.Join(", ", issues));
                    if (status != HealthStatus.Unhealthy)
                    {
                        description = $"🟡 Performance concerns: {string.Join(", ", issues)}";
                    }
                }

                return (description, status, data);
            }
            catch (Exception ex)
            {
                var errorData = new Dictionary<string, object>
                {
                    { "🚨_Error", ex.Message },
                    { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                };
                return ("❌ System performance check failed", HealthStatus.Unhealthy, errorData);
            }
        }

    }
}
