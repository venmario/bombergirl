using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BomberManStudentChallange.Classes
{
    public enum TipeBlok
    {
        NonDestructible,
        Destructible,
        Empty
    }
    public class Blok
    {
        public Control ObjekBlok { get; set; }
        public TipeBlok BlokTipe { get; set; }

        public Blok(Control pObjekBlok, TipeBlok pTipeBlok)
        {
            this.ObjekBlok = pObjekBlok;
            this.BlokTipe = pTipeBlok;
        }
    }
}
