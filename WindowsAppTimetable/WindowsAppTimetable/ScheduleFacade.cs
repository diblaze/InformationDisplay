﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WindowsAppTimetable.NovaSoftware;

namespace WindowsAppTimetable
{
    internal class ScheduleFacade
    {
        private const string SchoolId = "83020"; //ID for Platengymnasiet

        public static async Task PopulateScheduleIdsAsync(ObservableCollection<ScheduleID> ScheduleIds)
        {
            //Get IDs first
            var scheduleIdList = await GetScheduleIdsAsync();

            //add only the ones with both ID and Text - otherwise can not add to combolist.
            foreach (ScheduleID id in scheduleIdList.Where(id => id.ID != null && id.Text != null))
            {
                ScheduleIds.Add(id);
            }
        }

        private static async Task<ScheduleID[]> GetScheduleIdsAsync()
        {
            //Create new instance of NovaSoftware API client.
            var client = new ScheduleFileWebServiceSoapClient();

            //Download all IDs async.
            var scheduleIdList = await client.GetScheduleIDListAsync(SchoolId, 500);

            //Return list.
            return scheduleIdList;
        }
    }
}