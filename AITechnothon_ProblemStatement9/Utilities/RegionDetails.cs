using Amazon;

namespace AITechnothon_ProblemStatement9.Utilities
{
    public static class RegionDetails
    {
        public static RegionEndpoint GetRegionEndpoint(string? endpoint)
        {
            RegionEndpoint regionEndpoint;
            switch (endpoint)
            {
                case "APSouth1":
                    regionEndpoint = RegionEndpoint.APSouth1;
                    break;
                default:
                    regionEndpoint = RegionEndpoint.APSouth1;
                    break;
            }

            return regionEndpoint;
        }
    }
}
