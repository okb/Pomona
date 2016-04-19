﻿#region License

// Pomona is open source software released under the terms of the LICENSE specified in the
// project's repository, or alternatively at http://pomona.io/

#endregion

namespace Pomona.Example.Models.Existence
{
    public class GalaxyInfo : CelestialObject
    {
        public GalaxyInfo(Galaxy galaxy)
        {
            Galaxy = galaxy;
            Name = "info";
        }


        public Galaxy Galaxy { get; }
    }
}