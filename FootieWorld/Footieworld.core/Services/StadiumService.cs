using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Footieworld.core.Models;
using Footieworld.core.Services.Interfaces;
using FootieWorld.data.ef.UnitOfWork;

namespace Footieworld.core.Services
{
    public class StadiumService : IStadiumService
    {
        private IUnitOfWork _unitOfWork;
        public StadiumService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<string> GetAllStadiums()
        {
            var stadia = _unitOfWork.Context.tblStadiums.ToList();
            var list = new List<string>();
            foreach (var stadium in stadia)
            {
                list.Add(stadium.Name);
            }
            return list;
   
        }
    }
}
