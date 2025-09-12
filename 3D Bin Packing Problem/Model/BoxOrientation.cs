using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Bin_Packing_Problem.Model
{
    /// <summary>
    /// Box orientation
    /// </summary>
    public enum BoxOrientation
    {
        /// <summary>
        /// Width Height Length
        /// </summary>
        Whl,
        /// <summary>
        /// Width Length Height
        /// </summary>
        Wlh,
        /// <summary>
        /// Length Width Height
        /// </summary>
        Lwh,
        /// <summary>
        /// Length Height Width
        /// </summary>
        Lhw,
        /// <summary>
        /// Height Length Width
        /// </summary>
        Hlw,
        /// <summary>
        /// Height Width Length
        /// </summary>
        Hwl
    }
}

