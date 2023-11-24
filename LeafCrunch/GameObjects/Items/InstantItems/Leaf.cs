using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using LeafCrunch.Utilities.Sound;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    public class Leaf : InstantItem, IDrawable
    {
        protected int _pointIncrement = 10;
        
        virtual public int PointIncrement
        {
            get { return _pointIncrement; }
            set { _pointIncrement = value; }
        }

        private Image _image;
        override public Image CurrentImage { get { return _image; } }

        private System.Media.SoundPlayer _soundPlayer = new System.Media.SoundPlayer();

        /*public System.Media.SoundPlayer SoundPlayer
        {
            get
            {
                if (_soundPlayer == null)
                {
                    _soundPlayer = new System.Media.SoundPlayer();
                }
                if (SoundFile == null) return null;
                _soundPlayer.SoundLocation = SoundFile;
                return _soundPlayer;
            }
        }*/


         void Play(string audioPath)
         {
             MediaPlayer myPlayer = new MediaPlayer();
             var sysuri = new System.Uri(audioPath);
             myPlayer.Open(sysuri);
             myPlayer.Play();
         }


          public string SoundFile { get; set; }

        private SoundController _soundController;

        public SoundController SoundController
        {
            get { return _soundController; }
            set { _soundController = value; }
        }

        //create from item data loaded from file
        public Leaf (ItemData itemData) :base()
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.InstantItems.Leaf.Apply"))
                OperationMethodRegistry.TargetOperations.Add("Items.InstantItems.Leaf.Apply", Apply);

            ActivationKey = Keys.Enter;

            //this could probably go in the generic item code but whatever
            _image = UtilityMethods.ImageFromPath(itemData.SingleImage);
            X = itemData.X;
            Y = itemData.Y;
            W = _image.Width;
            H = _image.Height;
            _pointIncrement = itemData.PointIncrement;
            InitializeOperationFromRegistry(itemData.Operation);

            if (!string.IsNullOrEmpty(itemData.SoundFile))
            {
               // SoundFile = itemData.SoundFile;
                SoundController = new SoundController();
                SoundController.SoundURI = new System.Uri(Application.StartupPath + itemData.SoundFile);
            }

            //only consider it initialized if we came this way for now
            IsInitialized = true;
        }

        //probably this is where the crunch noise should go too idk i haven't thought this through
        virtual protected Result Apply(GenericGameObject genericGameObject, object paramList)
        {
            //lol why does it say victim here rename this you fool
            var victim = genericGameObject as IItemUser;
            if (victim != null)
            {
                victim.ApplyItem(paramList);
                //if (SoundPlayer != null)
                //{
                //       _soundPlayer.Play();
                //}
//                                if (SoundFile != null)
  //                            { 
    //                            Play(Application.StartupPath + "\\Sounds\\PointItems\\crunch.wav");
      //                    }
                if (SoundController != null) SoundController.Play(false);
            }
            return new Result() { Value = true };
        }
    }
}
