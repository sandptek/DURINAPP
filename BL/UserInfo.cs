using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
	public class UserInfo
	{
		public int orders = 0;
		public int items = 0;
		public int completedItems = 0;

		public UserInfo GetUserInfo(int userID, int hospitalID)
		{
			UserInfo userInfo = new UserInfo();
			using (var context = new DAL.DurinDBContext())
			{
				var user = context.Users.First(x => x.id == userID);

				userInfo.orders = context.Orders.Count(x => 
					!x.deleted 
					&& (x.hospital.id == hospitalID || hospitalID == 0) 
					&& ((user.type == Entities.DB.User.Type.patient && x.patient.id == userID) || (user.type == Entities.DB.User.Type.doctor && x.doctor.id == userID))
				);
				userInfo.items = context.OrderItems.Count(x => 
					!x.deleted 
					&& (x.order.hospital.id == hospitalID || hospitalID == 0) 
					&& !x.order.deleted 
					&& ((user.type == Entities.DB.User.Type.patient && x.order.patient.id == userID) || (user.type == Entities.DB.User.Type.doctor && x.order.doctor.id == userID))
				);
				userInfo.completedItems = context.OrderItems.Count(x => 
					!x.deleted 
					&& (x.order.hospital.id == hospitalID || hospitalID == 0) 
					&& !x.order.deleted 
					&& ((user.type == Entities.DB.User.Type.patient && x.order.patient.id == userID) || (user.type == Entities.DB.User.Type.doctor && x.order.doctor.id == userID)) 
					&& x.order.status == Entities.DB.Order.Status.done
				);
			}
			return userInfo;
		}
	}
}
