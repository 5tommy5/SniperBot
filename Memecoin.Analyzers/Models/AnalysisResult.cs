namespace Memecoin.Analyzers.Models
{
    public class AnalysisResult
    {
        public bool IsSafe { get; private set; }
        public string? Reason { get; private set; }

        public static AnalysisResult SafeResult() => new AnalysisResult { IsSafe = true };
        public static AnalysisResult UnSafeResult(string? reason = null) => new AnalysisResult { IsSafe = false, Reason = reason };
    }
}
