using Microsoft.Extensions.Diagnostics.HealthChecks;

using System.Diagnostics;
using System.Linq;


namespace GlobalConstants
{
    public static  class HealthInfoExtractor
    {
        public static (Dictionary<string, object> data, HealthStatus status, string description) ExtractMemoryUsageInfo(long totalMemory, long workingSet, Process process)
        {
            // Flatten the data structure for better UI display
            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    // Memory metrics (flattened for UI)
                    { "🧠_App_Heap_Memory_MB", Math.Round(totalMemory / 1024.0 / 1024.0, 2) },
                    { "🧠_Physical_RAM_Used_MB", Math.Round(workingSet / 1024.0 / 1024.0, 2) },
                    { "🧠_Private_Memory_MB", Math.Round(process.PrivateMemorySize64 / 1024.0 / 1024.0, 2) },
                    { "🧠_Virtual_Memory_MB", Math.Round(process.VirtualMemorySize64 / 1024.0 / 1024.0, 2) },
                    
                    // Garbage Collection
                    { "🗑️_GC_Gen0_Collections", GC.CollectionCount(0) },
                    { "🗑️_GC_Gen1_Collections", GC.CollectionCount(1) },
                    { "🗑️_GC_Gen2_Collections", GC.CollectionCount(2) },
                    
                    // Process Information
                    { "🧵_Thread_Count", process.Threads.Count },
                    { "🔧_Handle_Count", process.HandleCount },
                    { "🕒_Process_Start_Time", process.StartTime.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "⏱️_Process_Up_Time", (DateTime.Now - process.StartTime).ToString(@"dd\.hh\:mm\:ss") },
                    
                    // Detailed Explanations
                    { "📖_Heap_Explanation", "HEAP: Memory for your app's objects (strings, classes, data). Lower = more efficient." },
                    { "📖_RAM_Explanation", "RAM: Physical memory your process uses. This includes heap + .NET runtime + system overhead." },
                    { "📖_Private_Explanation", "PRIVATE: Total memory only your process can access. Includes heap + runtime + native memory." },
                    { "📖_Virtual_Explanation", "VIRTUAL: Total virtual address space. Usually much larger than physical memory." },
                    { "📖_Threads_Explanation", "THREADS: Execution units handling requests. Normal for web APIs to have 20-100 threads." },
                    { "📖_GC_Explanation", "GARBAGE COLLECTION: .NET automatically cleans up unused memory objects." },
                    
                    // What Your Numbers Mean
                    { "💡_Your_Heap_Status", "28MB heap is EXCELLENT - very memory efficient!" },
                    { "💡_Your_RAM_Status", "177MB RAM usage is NORMAL for a .NET web API" },
                    { "💡_Your_Thread_Status", "47 threads is HEALTHY for handling web requests" },
                    { "💡_Your_GC_Status", "GC pattern shows healthy memory management" },
                    
                    // Thresholds with Context
                    { "🟢_Healthy_Heap", "< 1000MB (You: 28MB - Excellent!)" },
                    { "🟡_Warning_Heap", "1000-2000MB" },
                    { "🔴_Critical_Heap", "> 2000MB" },
                    { "🟢_Healthy_Threads", "< 200 (You: 47 - Perfect!)" },
                    { "🟡_Warning_Threads", "200-500 threads" },
                    { "🔴_Critical_Threads", "> 500 threads" },
                    
                    // Performance Tips
                    { "💡_Performance_Tip", "Your memory usage is excellent! No optimization needed." },
                    { "💡_Monitoring_Tip", "Watch for heap growing over time - that could indicate memory leaks" },

                    { "🕒_Last_Checked", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                };
            var memoryMB = totalMemory / 1024.0 / 1024.0;
            var workingSetMB = workingSet / 1024.0 / 1024.0;
            var privateMemoryMB = process.PrivateMemorySize64 / 1024.0 / 1024.0;

            HealthStatus status = memoryMB > 2000 ? HealthStatus.Unhealthy :
                       memoryMB > 1000 ? HealthStatus.Degraded : HealthStatus.Healthy;

            // Enhanced description for main dashboard view
            string description = $"✅ Heap: {memoryMB:F1}MB (App Objects) | RAM: {workingSetMB:F1}MB (Physical Memory) | Private: {privateMemoryMB:F1}MB (Process Total) | Threads: {process.Threads.Count} (Request Handlers) | GC: {GC.CollectionCount(0)}/{GC.CollectionCount(1)}/{GC.CollectionCount(2)} (Cleanup Cycles)";

            return (data, status, description);

        }

    }
}
