using FootieWorld.data.ef;
using LoremNET;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootieWorldTests.TestUtils
{
    public static class UnitTestHelpers
    {
             public static Moq.Mock<FootieDbEntities> GetUnitTestDBContext()
        {
            var stadiums = new List<tblStadium>();
            var teams = new List<tblTeam>();

            for (int i = 0; i < 10; i++)
            {
                var stadium = CreateStadium();
                stadiums.Add(stadium);
                teams.Add(CreateTestTeam(stadium.StadiumID));
            }
            return MyMoqUtilities.MockDbContext(tblStadium: stadiums, tblteam: teams).DbContext;
        }

        private static tblTeam CreateTestTeam(Guid stadiumId)
        {
            var team = new tblTeam()
            {
                Name = Lorem.Words(1, true, false) + " FC",
                Address1 = Lorem.Words(3, true, false),
                Address2 = Lorem.Words(3, true, false),
                Address3 = Lorem.Words(3, true, false),
                Address4 = Lorem.Words(3, true, false),
                TeamId = Guid.NewGuid(),
                StadiumID = (Guid)stadiumId
            };
            return team;
        }

        private static tblStadium CreateStadium()
        {
            var stadium = new tblStadium()
            {
                Name = Lorem.Words(1, true, false),
                Address1 = Lorem.Words(3, true, false),
                Address2 = Lorem.Words(3, true, false),
                Address3 = Lorem.Words(3, true, false),
                Address4 = Lorem.Words(3, true, false),
                Capacity = (int)Lorem.Number(1000, 100000),
                StadiumID = Guid.NewGuid()
            };
            return stadium;
        }
    }
}
