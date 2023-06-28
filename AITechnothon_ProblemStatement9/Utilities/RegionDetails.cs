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

                case "USIsobEast1":
                    regionEndpoint = RegionEndpoint.USIsobEast1;
                    break;

                case "APEast1":
                    regionEndpoint = RegionEndpoint.APEast1;
                    break;

                case "AFSouth1":
                    regionEndpoint = RegionEndpoint.AFSouth1;
                    break;

                case "APNortheast1":
                    regionEndpoint = RegionEndpoint.APNortheast1;
                    break;

                case "APNortheast2":
                    regionEndpoint = RegionEndpoint.APNortheast2;
                    break;

                case "APNortheast3":
                    regionEndpoint = RegionEndpoint.APNortheast3;
                    break;

                case "APSouth2":
                    regionEndpoint = RegionEndpoint.APSouth2;
                    break;

                case "APSoutheast1":
                    regionEndpoint = RegionEndpoint.APSoutheast1;
                    break;

                case "APSoutheast2":
                    regionEndpoint = RegionEndpoint.APSoutheast2;
                    break;

                case "APSoutheast3":
                    regionEndpoint = RegionEndpoint.APSoutheast3;
                    break;

                case "APSoutheast4":
                    regionEndpoint = RegionEndpoint.APSoutheast4;
                    break;

                case "CACentral1":
                    regionEndpoint = RegionEndpoint.CACentral1;
                    break;

                case "EUCentral1":
                    regionEndpoint = RegionEndpoint.EUCentral1;
                    break;

                case "EUCentral2":
                    regionEndpoint = RegionEndpoint.EUCentral2;
                    break;

                case "EUNorth1":
                    regionEndpoint = RegionEndpoint.EUNorth1;
                    break;

                case "EUSouth1":
                    regionEndpoint = RegionEndpoint.EUSouth1;
                    break;

                case "EUSouth2":
                    regionEndpoint = RegionEndpoint.EUSouth2;
                    break;

                case "EUWest1":
                    regionEndpoint = RegionEndpoint.EUWest1;
                    break;

                case "EUWest2":
                    regionEndpoint = RegionEndpoint.EUWest2;
                    break;

                case "EUWest3 ":
                    regionEndpoint = RegionEndpoint.EUWest3;
                    break;

                case "MECentral1 ":
                    regionEndpoint = RegionEndpoint.MECentral1;
                    break;

                case "MESouth1 ":
                    regionEndpoint = RegionEndpoint.MESouth1;
                    break;

                case "SAEast1 ":
                    regionEndpoint = RegionEndpoint.SAEast1;
                    break;

                case "USEast1 ":
                    regionEndpoint = RegionEndpoint.USEast1;
                    break;

                case "USEast2 ":
                    regionEndpoint = RegionEndpoint.USEast2;
                    break;

                case "USWest1 ":
                    regionEndpoint = RegionEndpoint.USWest1;
                    break;

                case "USWest2 ":
                    regionEndpoint = RegionEndpoint.USWest2;
                    break;

                case "CNNorth1 ":
                    regionEndpoint = RegionEndpoint.CNNorth1;
                    break;

                case "CNNorthWest1 ":
                    regionEndpoint = RegionEndpoint.CNNorthWest1;
                    break;

                case "USGovCloudEast1 ":
                    regionEndpoint = RegionEndpoint.USGovCloudEast1;
                    break;

                case "USGovCloudWest1 ":
                    regionEndpoint = RegionEndpoint.USGovCloudWest1;
                    break;

                case "USIsoEast1  ":
                    regionEndpoint = RegionEndpoint.USIsobEast1;
                    break;

                case "USIsoWest1  ":
                    regionEndpoint = RegionEndpoint.USIsoWest1;
                    break;

                default:
                    regionEndpoint = RegionEndpoint.APSouth1;
                    break;
            }

            return regionEndpoint;
        }
    }
}