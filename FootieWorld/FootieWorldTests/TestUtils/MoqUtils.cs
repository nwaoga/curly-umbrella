using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coderful.EntityFramework.Testing.Mock;
using FootieWorld.data.ef;
using Moq;

namespace FootieWorldTests.TestUtils
{
    internal static class MyMoqUtilities
    {
        public static MockedDbContext<FootieDbEntities> MockDbContext(IList<tblStadium> tblStadium ,IList<tblTeam> tblteam)
        {

            var mockContext = new Mock<FootieDbEntities>();
            

            // Create the DbSet objects.
            var dbSets = new object[]
            {
            MoqUtilities.MockDbSet(tblStadium, (objects, stadium) => stadium.StadiumID == (Guid)objects[10]),
            MoqUtilities.MockDbSet(tblteam, (objects, team) => team.TeamId == (Guid)objects[10])
            };            

            return new MockedDbContext<FootieDbEntities>(mockContext, dbSets);
        }
    }
}
