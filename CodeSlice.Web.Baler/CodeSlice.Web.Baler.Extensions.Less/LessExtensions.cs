
namespace CodeSlice.Web.Baler.Extensions.Less
{
    // Provides extension methods for converting 
    // [Less](http://www.dotlesscss.org/) files into CSS
    internal static class LessExtensions
    {
        // Converts a less bale to CSS and outputs as CSS source
        public static string AsLess(this IBale bale)
        {
            return bale.After(dotless.Core.Less.Parse).AsCss();
        }
    }
}
