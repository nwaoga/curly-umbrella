using FootieWorld.data.ef;
using LoremNET;
using System;

namespace FootieWorldTests
{
    public class Testbase
    {
      public FootieDbEntities1 FootieDatabase { get; set; }


     public Testbase()
        {
 
        }

      public void reseedDb ()
      {
            //Remove all entries
            this.FootieDatabase.tblTeams.RemoveRange(FootieDatabase.tblTeams);
            this.FootieDatabase.tblStadiums.RemoveRange(FootieDatabase.tblStadiums);

            
            for (int i = 0; i < 10; i++)
            {
                var stadium = createStadium();
                this.FootieDatabase.tblStadiums.Add(stadium);
                this.FootieDatabase.tblTeams.Add(createTestTeam(stadium.StadiumID));
            }
            this.FootieDatabase.SaveChanges();

        }
        private tblTeam createTestTeam(Guid stadiumId)
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

        private tblStadium createStadium()
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