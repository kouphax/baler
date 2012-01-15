using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSlice.Web.Baler.Extensions.CssMedia
{
    // `CssMediaExtensions` provide methods for setting the media type that a 
    // stylesheet should render for such as `screen` or `print` 
    public static class CssMediaExtensions
    {
        // The `WithMedia` Extension allows you to set the media attribute of 
        // the rendered CSS tag.
        public static IBale WithMedia(this IBale bale, string media)
        {
            return bale.Attr("media", media);
        }

        // `AsCss` overrides the existing `AsCss` method allowing you to pass a 
        // media property directly into the render call
        public static string AsCss(this IBale bale, string media)
        {
            return bale.WithMedia(media).AsCss();
        }
    }
}
