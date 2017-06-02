using System;

namespace Nelibur.ObjectMapper
{
    /// <summary>
    ///     Configuration for TinyMapper
    /// </summary>
    public interface ITinyMapperConfig
    {
        /// <summary>
        ///     Allow mapping from a base class if a mapping for something more specific is unavailable
        /// </summary>
        bool EnablePolymorphicMapping { get; set; }

        /// <summary>
        ///     Create an automatic binding based on property names
        /// </summary>
        bool EnableAutoBinding { get; set; }

        /// <summary>
        ///     Custom name matching function used for auto bindings
        /// </summary>
        /// <param name="nameMatching">Function to match names</param>
        void NameMatching(Func<string, string, bool> nameMatching);

        /// <summary>
        ///     Reset settings to default
        /// </summary>
        void Reset();
    }
}
