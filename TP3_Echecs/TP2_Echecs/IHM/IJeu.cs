using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.Echecs;

namespace TP2_Echecs.IHM
{
    public interface IJeu
    {
        IEvenements vue { get; set; }

        void CommencerPartie();

        void DeplacerPiece(int x_depart, int y_depart, int x_arrivee, int y_arrivee);

        Case GetCase(int x, int y);

        void Undo();

        void Redo();
    }
}
