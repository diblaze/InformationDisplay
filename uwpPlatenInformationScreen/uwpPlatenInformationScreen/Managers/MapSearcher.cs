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
        /// The rooms collection
        /// </summary>
        private static readonly List<string> RoomsCollection = new List<string>
                                                               {
                                                                   "D200",
                                                                   "D201",
                                                                   "D202",
                                                                   "D203",
                                                                   "D204",
                                                                   "D205",
                                                                   "D206",
                                                                   "D207",
                                                                   "D208",
                                                                   "D209",
                                                                   "D210",
                                                                   "D211",
                                                                   "D212",
                                                                   "D213",
                                                                   "D214",
                                                                   "D215",
                                                                   "C200",
                                                                   "C201",
                                                                   "C202",
                                                                   "C203",
                                                                   "C204",
                                                                   "C205",
                                                                   "C206",
                                                                   "C207",

                                                               };

        /// <summary>
        ///     Searches for room asynchronous.
        /// </summary>
        /// <param name="room">The room searched for.</param>
        /// <returns>All rooms matching the inputed text</returns>
        public static IEnumerable<string> SearchForRoomAsync(string room)
        {
            return RoomsCollection.Where(r => r.IndexOf(room, StringComparison.CurrentCultureIgnoreCase) > -1);
        }

        /// <summary>
        /// Gets the room image.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns></returns>
        public static async Task<BitmapImage> GetRoomImage(string room)
        {
            //image
            BitmapImage roomImage = null;

            //check user inputted room to figure out which house to search in
            string houseToPullPicture = room.Substring(0, 1);

            switch (houseToPullPicture)
            {
                case "A":
                    roomImage = await FindRoom("A", room.Substring(1));
                    break;
                case "B":
                    roomImage = await FindRoom("B", room.Substring(1));
                    break;
                case "C":
                    roomImage = await FindRoom("C", room.Substring(1));
                    break;
                case "D":
                    roomImage = await FindRoom("D", room.Substring(1));
                    break;
                default:
                    break;
            }

            return roomImage;
        }

        /// <summary>
        /// Finds the room image.
        /// </summary>
        /// <param name="floor">The floor.</param>
        /// <param name="number">The number.</param>
        /// <returns><c>BitmapImage</c> of the room image</returns>
        private static async Task<BitmapImage> FindRoom(string floor, string number)
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
                case "B":
                    imageFolder = (StorageFolder) await installedLocation.TryGetItemAsync(@"Images\Floors\B");
                    break;
                case "C":
                    imageFolder = (StorageFolder) await installedLocation.TryGetItemAsync(@"Images\Floors\C");
                    break;
                case "D":
                    imageFolder = (StorageFolder) await installedLocation.TryGetItemAsync(@"Images\Floors\D");
                    break;
            }

            //does an image for the room exist?
            StorageFile imageFile = (StorageFile) await imageFolder?.TryGetItemAsync(floor + number + ".jpg");

            //if not
            if (imageFile == null)
            {
                //show error message
                MessageDialog errorDialog = new MessageDialog("Kunde inte hitta sal!", "Fel");
                await errorDialog.ShowAsync();
                return null;
            }
            
            //read and create a new bitmapimage from the room image in folder.
            var image = new BitmapImage(new Uri(imageFolder?.Path + @"\" + floor+number + ".jpg"));
                
            return image;
        }

        public static async Task<BitmapImage> GetFloorImage(string house)
        {
            //image
            BitmapImage floorImage = null;

            //check user inputted room to figure out which house to search in

            switch (house)
            {
                case "A":
                    floorImage = await FindFloor("A");
                    break;
                case "B":
                    floorImage = await FindFloor("B");
                    break;
                case "C":
                    floorImage = await FindFloor("C");
                    break;
                case "D":
                    floorImage = await FindFloor("D");
                    break;
                default:
                    break;
            }

            return floorImage;
        }

        private static async Task<BitmapImage> FindFloor(string floor)
        {
            //find install location to find our images
            StorageFolder installedLocation = Package.Current.InstalledLocation;
            //holder for our image folder
            StorageFolder imageFolder = (StorageFolder)await installedLocation.TryGetItemAsync(@"Images\Floors");

            //does an image for the room exist?
            StorageFile imageFile = (StorageFile)await imageFolder?.TryGetItemAsync(floor + ".jpg");

            //if not
            if (imageFile == null)
            {
                //show error message
                MessageDialog errorDialog = new MessageDialog("Kunde inte hitta våning!", "Fel");
                await errorDialog.ShowAsync();
                return null;
            }

            //read and create a new bitmapimage from the room image in folder.
            var image = new BitmapImage(new Uri(imageFolder?.Path + @"\" + floor + ".jpg"));

            return image;
        }
    }
}