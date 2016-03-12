using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace uwpPlatenInformationScreen.Managers
{
    internal class MapSearcher
    {
        /// <summary>
        ///     The rooms collection
        /// </summary>
        private static readonly List<string> RoomsCollection = new List<string>
                                                               {
                                                                   "C205",
                                                                   "C206",
                                                                   "C207",
                                                                   "C212",
                                                                   "C213",
                                                                   "C217",
                                                                   "C218",
                                                                   "C219",
                                                                   "C221",
                                                                   "C222",
                                                                   "C223",
                                                                   "C227",
                                                                   "C228",
                                                                   "C230",
                                                                   "C403",
                                                                   "C404",
                                                                   "C405",
                                                                   "C409",
                                                                   "C410",
                                                                   "C411",
                                                                   "C412",
                                                                   "C414",
                                                                   "C430",
                                                                   "C432",
                                                                   "C433",
                                                                   "C434",
                                                                   "C435",
                                                                   "C437",
                                                                   "D203",
                                                                   "D206",
                                                                   "D207",
                                                                   "D209",
                                                                   "D216",
                                                                   "D217",
                                                                   "D218",
                                                                   "D221",
                                                                   "D230",
                                                                   "D231",
                                                                   "D232",
                                                                   "D234",
                                                                   "D235",
                                                                   "D236",
                                                                   "D237",
                                                                   "D238",
                                                                   "D240",
                                                                   "D243",
                                                                   "D245",
                                                                   "D245",
                                                                   "D246",
                                                                   "D248",
                                                                   "D249",
                                                                   "D251",
                                                                   "D252",
                                                                   "D253",
                                                                   "D254",
                                                                   "D256",
                                                                   "D257",
                                                                   "D259",
                                                                   "D260",
                                                                   "D261",
                                                                   "D270",
                                                                   "D273",
                                                                   "E403",
                                                                   "E404",
                                                                   "E407",
                                                                   "E408"
                                                               };

        /// <summary>
        ///     Searches for room.
        /// </summary>
        /// <param name="room">The room searched for.</param>
        /// <returns>All rooms matching the inputed text</returns>
        public static IEnumerable<string> SearchForRoomAsync(string room)
        {
            return RoomsCollection.Where(r => r.IndexOf(room, StringComparison.CurrentCultureIgnoreCase) > -1);
        }

        /// <summary>
        ///     Gets the room image.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns></returns>
        public static async Task<BitmapImage> GetRoomImageAsync(string room)
        {
            //image
            BitmapImage roomImage = null;

            //check user inputted room to figure out which house to search in
            string houseToPullPicture = room.Substring(0, 1);

            switch (houseToPullPicture)
            {
                case "A":
                    roomImage = await FindRoomAsync("A", room.Substring(1));
                    break;

                case "C":
                    roomImage = await FindRoomAsync("C", room.Substring(1));
                    break;
                case "D":
                    roomImage = await FindRoomAsync("D", room.Substring(1));
                    break;
                case "E":
                    roomImage = await FindRoomAsync("E", room.Substring(1));
                    break;
                default:
                    break;
            }

            return roomImage;
        }

        /// <summary>
        ///     Finds the room image.
        /// </summary>
        /// <param name="floor">The floor.</param>
        /// <param name="number">The number.</param>
        /// <returns><c>BitmapImage</c> of the room image</returns>
        private static async Task<BitmapImage> FindRoomAsync(string floor, string number)
        {
            //find install location to find our images
            StorageFolder installedLocation = Package.Current.InstalledLocation;
            //holder for our image folder
            StorageFolder imageFolder = null;

            //which floor?
            switch (floor)
            {
                case "A":
                    imageFolder = (StorageFolder) await installedLocation.TryGetItemAsync(@"Images\Floors\A");
                    break;

                case "C":
                    imageFolder = (StorageFolder) await installedLocation.TryGetItemAsync(@"Images\Floors\C");
                    break;
                case "D":
                    imageFolder = (StorageFolder) await installedLocation.TryGetItemAsync(@"Images\Floors\D");
                    break;
                case "E":
                    imageFolder = (StorageFolder) await installedLocation.TryGetItemAsync(@"Images\Floors\E");
                    break;
            }

            //does an image for the room exist?
            var imageFile = (StorageFile) await imageFolder?.TryGetItemAsync(floor + number + ".jpg");

            //if not
            if (imageFile == null)
            {
                //show error message
                var errorDialog = new MessageDialog("Kunde inte hitta sal!", "Fel");
                await errorDialog.ShowAsync();
                return null;
            }

            //read and create a new bitmapimage from the room image in folder.
            var image = new BitmapImage(new Uri(imageFolder?.Path + @"\" + floor + number + ".jpg"));

            return image;
        }

        public static async Task<BitmapImage> GetFloorImageAsync(string house, string floor)
        {
            //image
            BitmapImage floorImage = null;

            //check user inputted room to figure out which house to search in

            switch (house)
            {
                case "A":
                    floorImage = await FindFloor(floor);
                    break;

                case "C":
                    floorImage = await FindFloor(floor);
                    break;
                case "D":
                    floorImage = await FindFloor(floor);
                    break;
                case "E":
                    floorImage = await FindFloor(floor);
                    break;
                default:
                    break;
            }

            return floorImage;
        }

        /// <summary>
        ///     Finds the floor.
        /// </summary>
        /// <param name="floor">The floor.</param>
        /// <returns>The floor image.</returns>
        private static async Task<BitmapImage> FindFloor(string floor)
        {
            //find install location to find our images
            StorageFolder installedLocation = Package.Current.InstalledLocation;
            //holder for our image folder
            var imageFolder = (StorageFolder) await installedLocation.TryGetItemAsync(@"Images\Floors");

            //does an image for the room exist?
            var imageFile = (StorageFile) await imageFolder?.TryGetItemAsync(floor + ".jpg");

            //if not
            if (imageFile == null)
            {
                //show error message
                var errorDialog = new MessageDialog("Kunde inte hitta våning!", "Fel");
                await errorDialog.ShowAsync();
                return null;
            }

            //read and create a new bitmapimage from the room image in folder.
            var image = new BitmapImage(new Uri(imageFolder?.Path + @"\" + floor + ".jpg"));

            return image;
        }
    }
}