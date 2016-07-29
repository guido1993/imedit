using System.Runtime.Serialization;

namespace Imedit.Models
{
    public enum FilterEnum
    {
        [EnumMember (Value =  "Gray Scale")]
        GrayScale,
        Flip,
        Rotate,
        Lighten,
        Darken,
        Contrast,
        Gamma,
        Invert,
        None
    }
}