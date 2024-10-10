using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    public class Partie : IJeu
    {
        public IEvenements vue
        {
            get { return _vue; }
            set {_vue = value; }
        }

        StatusPartie status
        {
            get { return _status; }
            set
            {
                _status = value;
                vue.ActualiserPartie(_status);
            }
        }


        /* attributs */

        StatusPartie _status = StatusPartie.Reset;
        List<Coup> CoupListe = new List<Coup>();
        int IndexCurrentCoup;
        bool undo = false;
        bool ChangeState = false;


        /* associations */

        IEvenements _vue;
        public Joueur blancs;
        public Joueur noirs;
        public Echiquier echiquier;  


        /* methodes */

        public void CommencerPartie()
        {
            //Pour nouvelle Patie
            IndexCurrentCoup = -1;
            if(blancs != null)
            {
                CoupListe.Clear();
                vue.ActualiserHistoric(null);
                noirs.deadPiece.Clear();
                blancs.deadPiece.Clear();
                vue.ActualiserCaptures(null);
                foreach (Piece p in blancs.pieces)
                {
                    if(p.position != null)
                    {
                        p.position.UnLink(p);
                    }
                }
                blancs.pieces.Clear();
                foreach (Piece p in noirs.pieces)
                {
                    if (p.position != null)
                    {
                        p.position.UnLink(p);
                    }
                }
                noirs.pieces.Clear();
                
            }            
            // creation des joueurs
            blancs = new Joueur(this, CouleurCamp.Blanche);
            noirs = new Joueur(this, CouleurCamp.Noire);

            // creation de l'echiquier
            echiquier = new Echiquier(this);

            // placement des pieces
            blancs.PlacerPieces(echiquier);  
            noirs.PlacerPieces(echiquier);  

            // initialisation de l'état
            status = StatusPartie.TraitBlancs;         
        }

        public void DeplacerPiece(int x_depart, int y_depart, int x_arrivee, int y_arrivee)
        {
            // case de départ
            Case depart = echiquier.cases[x_depart, y_depart];

            // case d'arrivée
            Case destination = echiquier.cases[x_arrivee, y_arrivee];
            
            // deplacer            
            bool ok = depart.piece.Deplacer(destination);

            // changer d'état
            if (ok)
            {
                StatusChange();
            }            
        }     
        
        public void StatusChange()
        {
            bool NulleCoup = Plus50CoupNull();
            listCaseDestinationAllPiece();
            RestartPrévisualisation();
            DeadPiece();
            status = MiseEnEchec();
            if (NulleCoup == false)
            {
                if (status.etat == EtatPartie.Echec)
                {
                    bool Moove = VerifieSiOnPeutMoove();
                    if (Moove)
                    {
                        bool EviteMat = VerifiEviterMat();
                        if (!EviteMat)
                        {
                            if (status.couleur == CouleurCamp.Blanche)
                            {
                                status = StatusPartie.MatBlancs;
                            }
                            else
                            {
                                status = StatusPartie.MatNoirs;
                            }
                        }
                    }
                    else
                    {
                        if (status.couleur == CouleurCamp.Blanche)
                        {
                            status = StatusPartie.MatBlancs;
                        }
                        else
                        {
                            status = StatusPartie.MatNoirs;
                        }
                    }
                }
                else
                {
                    bool NulPartie = partieNull();
                    if (NulPartie)
                    {
                        status = StatusPartie.Nul;
                    }
                    else
                    {

                        if (ChangeState == false) ChangerEtat();
                        else ChangeState = false;
                    }
                }
            }
            else status = StatusPartie.Nul;
        }
                       

        void ChangerEtat(bool echec = false, bool mat = false)
        {
            if (status == StatusPartie.TraitBlancs)
            {
                status = StatusPartie.TraitNoirs;
            }
            else
            {
                status = StatusPartie.TraitBlancs;
            }            
        }

        public void DeadPiece()
        {
            List<InfoPiece> deadPieceBlanche = blancs.deadPiece;
            List<InfoPiece> deadPieceNoir = noirs.deadPiece;  
            List<InfoPiece> deadPiece = new List<InfoPiece>();
            foreach(InfoPiece ip in deadPieceBlanche)
            {
                deadPiece.Add(ip);
            }
            foreach (InfoPiece ip in deadPieceNoir)
            {
                deadPiece.Add(ip);
            }
            vue.ActualiserCaptures(deadPiece);
        }

        public void AddCoup(Piece piece,Case arriver, Case depart, Piece deadPiece, bool RockLeft, bool RockRight, bool PrisePassante,bool Prom)
        {              
            if(undo == true)
            {
                MooveCleanListCoup();
                undo = false;
            }
            IndexCurrentCoup++;
            Coup nouveaucoup = new Coup(piece, arriver, depart, deadPiece, status, RockLeft, RockRight, PrisePassante, Prom, IndexCurrentCoup);
            CoupListe.Add(nouveaucoup);  
            vue.ActualiserHistoric(nouveaucoup);
        }

        public Case GetCase(int x, int y)
        {
            return echiquier.cases[x, y];
        }

        public void RestartPrévisualisation()
        {            
            List<Piece> blanchePiece = blancs.pieces;
            List<Piece> noirPiece = noirs.pieces;

            foreach (Piece pb in blanchePiece)
            {
                pb.listCaseDestination = pb.Destiny();
            }

            foreach (Piece pn in noirPiece)
            {
                pn.listCaseDestination = pn.Destiny();
            }            
        }

        //Methode pour definir les cases où le roi ne peut se faire Manger
        public void listCaseDestinationAllPiece()
        {
            List<Piece> blanchePiece = blancs.pieces;
            List<Piece> noirPiece = noirs.pieces;
            blancs.destinationAllPiece.Clear();
            noirs.destinationAllPiece.Clear();           
            foreach (Piece pb in blanchePiece)
            {                
                if(pb.Alive == true)
                {
                    if (pb.info.type == TypePiece.Pion)
                    {
                        if (pb.position != null)
                        {
                            if ((pb.position.colonne + 1 < 8 && pb.position.colonne + 1 > 0) &&
                           (pb.position.ligne - 1 < 8 && pb.position.ligne - 1 >= 0))
                            {
                                Case next = echiquier.cases[pb.position.colonne + 1, pb.position.ligne - 1];
                                int jean = 0;
                                foreach (Case ab in blancs.destinationAllPiece)
                                {
                                    if (ab != next) jean++; ;
                                }
                                if (jean == blancs.destinationAllPiece.Count()) blancs.destinationAllPiece.Add(next);
                            }
                            if ((pb.position.colonne - 1 < 8 && pb.position.colonne - 1 > 0) &&
                               (pb.position.ligne - 1 < 8 && pb.position.ligne - 1 >= 0))
                            {
                                Case next2 = echiquier.cases[pb.position.colonne - 1, pb.position.ligne - 1];
                                int jean2 = 0;
                                foreach (Case ab in blancs.destinationAllPiece)
                                {
                                    if (ab != next2) jean2++; ;
                                }
                                if (jean2 == blancs.destinationAllPiece.Count()) blancs.destinationAllPiece.Add(next2);
                            }
                        }
                    }
                    else if (pb.info.type == TypePiece.Roi)
                    {
                        if (pb.position != null)
                        {                            
                            if ((pb.position.colonne + 1 < 8 && pb.position.colonne + 1 > 0) &&
                            (pb.position.ligne + 1 < 8 && pb.position.ligne + 1 >= 0))
                            {
                                Case next1 = echiquier.cases[pb.position.colonne + 1, pb.position.ligne + 1];
                                if (next1.piece == null || next1.piece.info.couleur == CouleurCamp.Noire) blancs.destinationAllPiece.Add(next1);
                            }
                            if ((pb.position.colonne + 1 < 8 && pb.position.colonne + 1 > 0) &&
                            (pb.position.ligne - 1 < 8 && pb.position.ligne - 1 >= 0))
                            {
                                Case next2 = echiquier.cases[pb.position.colonne + 1, pb.position.ligne - 1];
                                if (next2.piece == null || next2.piece.info.couleur == CouleurCamp.Noire) blancs.destinationAllPiece.Add(next2);

                            }
                            if (pb.position.colonne + 1 < 8 && pb.position.colonne + 1 > 0)
                            {
                                Case next3 = echiquier.cases[pb.position.colonne + 1, pb.position.ligne];
                                if (next3.piece == null || next3.piece.info.couleur == CouleurCamp.Noire) blancs.destinationAllPiece.Add(next3);
                                   
                            }
                            if ((pb.position.colonne - 1 < 8 && pb.position.colonne - 1 > 0) &&
                                (pb.position.ligne + 1 < 8 && pb.position.ligne + 1 >= 0))
                            {
                                Case next4 = echiquier.cases[pb.position.colonne - 1, pb.position.ligne + 1];
                                if (next4.piece == null || next4.piece.info.couleur == CouleurCamp.Noire) blancs.destinationAllPiece.Add(next4);                          
                            }
                            if ((pb.position.colonne - 1 < 8 && pb.position.colonne - 1 > 0) &&
                                (pb.position.ligne - 1 < 8 && pb.position.ligne - 1 >= 0))
                            {
                                Case next5 = echiquier.cases[pb.position.colonne - 1, pb.position.ligne - 1];
                                if (next5.piece == null || next5.piece.info.couleur == CouleurCamp.Noire) blancs.destinationAllPiece.Add(next5);
                            }
                            if (pb.position.colonne - 1 < 8 && pb.position.colonne - 1 > 0)
                            {
                                Case next6 = echiquier.cases[pb.position.colonne - 1, pb.position.ligne];
                                if (next6.piece == null || next6.piece.info.couleur == CouleurCamp.Noire) blancs.destinationAllPiece.Add(next6);                                
                                    
                            }
                            if (pb.position.ligne + 1 < 8 && pb.position.ligne + 1 >= 0)
                            {
                                Case next7 = echiquier.cases[pb.position.colonne, pb.position.ligne + 1];
                                if (next7.piece == null || next7.piece.info.couleur == CouleurCamp.Noire) blancs.destinationAllPiece.Add(next7);                               
                            }
                            if (pb.position.ligne - 1 < 8 && pb.position.ligne - 1 >= 0)
                            {
                                Case next8 = echiquier.cases[pb.position.colonne, pb.position.ligne - 1];
                                if (next8.piece == null || next8.piece.info.couleur == CouleurCamp.Noire) blancs.destinationAllPiece.Add(next8);                                                              
                            }
                        }
                    }
                    else
                    {
                        if (pb.position != null)
                        {
                            foreach (Case c in pb.listCaseDestination)
                            {
                                int jean = 0;
                                foreach (Case ab in blancs.destinationAllPiece)
                                {
                                    if (ab != c) jean++;
                                }
                                if (jean == blancs.destinationAllPiece.Count())
                                {
                                    blancs.destinationAllPiece.Add(c);
                                }
                            }
                        }                            
                    }
                }                               
            }

            foreach (Piece pb in noirPiece)
            {
                int piecedead = 0;
                foreach (InfoPiece pd in noirs.deadPiece)
                {
                    if (pd == pb.info) piecedead = 1;
                }
                if (piecedead == 0)
                {
                    if (pb.info.type == TypePiece.Pion)
                    {
                        if (pb.position != null)
                        {
                            if ((pb.position.colonne + 1 < 8 && pb.position.colonne + 1 > 0) &&
                            (pb.position.ligne + 1 < 8 && pb.position.ligne + 1 >= 0))
                            {
                                Case next = echiquier.cases[pb.position.colonne + 1, pb.position.ligne + 1];
                                int jean = 0;
                                foreach (Case ab in noirs.destinationAllPiece)
                                {
                                    if (ab != next) jean++; ;
                                }
                                if (jean == noirs.destinationAllPiece.Count()) noirs.destinationAllPiece.Add(next);
                            }
                            if ((pb.position.colonne - 1 < 8 && pb.position.colonne - 1 > 0) &&
                               (pb.position.ligne + 1 < 8 && pb.position.ligne + 1 >= 0))
                            {
                                Case next2 = echiquier.cases[pb.position.colonne - 1, pb.position.ligne + 1];
                                int jean2 = 0;
                                foreach (Case ab in noirs.destinationAllPiece)
                                {
                                    if (ab != next2) jean2++; ;
                                }
                                if (jean2 == noirs.destinationAllPiece.Count()) noirs.destinationAllPiece.Add(next2);
                            }
                        }
                    }
                    else if (pb.info.type == TypePiece.Roi)
                    {
                        if(pb.position != null)
                        {
                            if((pb.position.colonne + 1 < 8 && pb.position.colonne + 1 > 0) &&
                            (pb.position.ligne + 1 < 8 && pb.position.ligne + 1 >= 0))
                            {
                                Case next1 = echiquier.cases[pb.position.colonne + 1, pb.position.ligne + 1];
                                if (next1.piece == null || next1.piece.info.couleur == CouleurCamp.Blanche) noirs.destinationAllPiece.Add(next1);
                            }
                            if ((pb.position.colonne + 1 < 8 && pb.position.colonne + 1 > 0) &&
                            (pb.position.ligne - 1 < 8 && pb.position.ligne - 1 >= 0))
                            {
                                Case next2 = echiquier.cases[pb.position.colonne + 1, pb.position.ligne - 1];
                                if (next2.piece == null || next2.piece.info.couleur == CouleurCamp.Blanche) noirs.destinationAllPiece.Add(next2);
                            }
                            if (pb.position.colonne + 1 < 8 && pb.position.colonne + 1 > 0)
                            {
                                Case next3 = echiquier.cases[pb.position.colonne + 1, pb.position.ligne];
                                if (next3.piece == null || next3.piece.info.couleur == CouleurCamp.Blanche) noirs.destinationAllPiece.Add(next3);
                            }
                            if ((pb.position.colonne - 1 < 8 && pb.position.colonne - 1 > 0) &&
                                (pb.position.ligne + 1 < 8 && pb.position.ligne + 1 >= 0))
                            {
                                Case next4 = echiquier.cases[pb.position.colonne - 1, pb.position.ligne + 1];
                                if (next4.piece == null || next4.piece.info.couleur == CouleurCamp.Blanche) noirs.destinationAllPiece.Add(next4);
                            }
                            if ((pb.position.colonne - 1 < 8 && pb.position.colonne - 1 > 0) &&
                                (pb.position.ligne - 1 < 8 && pb.position.ligne - 1 >= 0))
                            {
                                Case next5 = echiquier.cases[pb.position.colonne - 1, pb.position.ligne - 1];
                                if (next5.piece == null || next5.piece.info.couleur == CouleurCamp.Blanche) noirs.destinationAllPiece.Add(next5);
                            }
                            if (pb.position.colonne - 1 < 8 && pb.position.colonne - 1 > 0)
                            {
                                Case next6 = echiquier.cases[pb.position.colonne - 1, pb.position.ligne];
                                if (next6.piece == null || next6.piece.info.couleur == CouleurCamp.Blanche) noirs.destinationAllPiece.Add(next6);
                            }
                            if (pb.position.ligne + 1 < 8 && pb.position.ligne + 1 >= 0)
                            {
                                Case next7 = echiquier.cases[pb.position.colonne, pb.position.ligne + 1];
                                if (next7.piece == null || next7.piece.info.couleur == CouleurCamp.Blanche) noirs.destinationAllPiece.Add(next7);
                            }
                            if (pb.position.ligne - 1 < 8 && pb.position.ligne - 1 >= 0)
                            {
                                Case next8 = echiquier.cases[pb.position.colonne, pb.position.ligne - 1];
                                if (next8.piece == null || next8.piece.info.couleur == CouleurCamp.Blanche) noirs.destinationAllPiece.Add(next8);
                            }    
                        }  
                    }
                    else
                    {
                        if (pb.position != null)
                        {
                            foreach (Case c in pb.listCaseDestination)
                            {
                                int jean = 0;
                                foreach (Case ab in noirs.destinationAllPiece)
                                {
                                    if (ab != c) jean++;
                                }
                                if (jean == noirs.destinationAllPiece.Count())
                                {
                                    noirs.destinationAllPiece.Add(c);
                                }
                            }
                        }                            
                    }
                }                                    
            }
        }

        public void Undo()
        {
            if(IndexCurrentCoup >= 0)
            {
                CoupListe[IndexCurrentCoup].piece.StopPriseEnPassant();
                if (CoupListe[IndexCurrentCoup].Promotion == false)
                {
                    CoupListe[IndexCurrentCoup].depart.Link(CoupListe[IndexCurrentCoup].piece);
                }
                else
                {
                    if (CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Blanche)
                    {
                        blancs.pieces.Remove(blancs.pieces[15]);
                        blancs.pieces.Add(CoupListe[IndexCurrentCoup].piece);
                        CoupListe[IndexCurrentCoup].depart.Link(blancs.pieces[15]);
                    }
                    else
                    {
                        noirs.pieces.Remove(noirs.pieces[15]);
                        noirs.pieces.Add(CoupListe[IndexCurrentCoup].piece);
                        CoupListe[IndexCurrentCoup].depart.Link(noirs.pieces[15]);
                    }
                }
                status = CoupListe[IndexCurrentCoup].state;
                if (CoupListe[IndexCurrentCoup].deadPiece != null)
                {
                    if(CoupListe[IndexCurrentCoup].priseEnPassante == false)
                    {
                        CoupListe[IndexCurrentCoup].arriver.Link(CoupListe[IndexCurrentCoup].deadPiece);                        
                    }
                    else
                    {
                        CoupListe[IndexCurrentCoup].deadPiece.position.Link(CoupListe[IndexCurrentCoup].deadPiece);
                        CoupListe[IndexCurrentCoup].deadPiece.SetTruePriseEnPassant();
                    }

                    if (CoupListe[IndexCurrentCoup].deadPiece.info.couleur == CouleurCamp.Blanche)
                    {

                        for (int i = 0; i < blancs.deadPiece.Count(); i++)
                        {
                            if (blancs.deadPiece[i] == CoupListe[IndexCurrentCoup].deadPiece.info)
                            {
                                blancs.deadPiece.Remove(blancs.deadPiece[i]);
                                vue.ActualiserCaptures(null);
                                DeadPiece();

                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < noirs.deadPiece.Count(); i++)
                        {
                            if (noirs.deadPiece[i] == CoupListe[IndexCurrentCoup].deadPiece.info)
                            {
                                noirs.deadPiece.Remove(noirs.deadPiece[i]);
                                vue.ActualiserCaptures(null);
                                DeadPiece();
                                break;
                            }
                        }
                    }
                }    
                if(CoupListe[IndexCurrentCoup].piece.info.type ==TypePiece.Pion && CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Blanche)
                {
                    if(CoupListe[IndexCurrentCoup].depart.ligne == 6)
                    {                       
                        CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                    }
                }
                if (CoupListe[IndexCurrentCoup].piece.info.type == TypePiece.Pion && CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Noire)
                {
                    if (CoupListe[IndexCurrentCoup].depart.ligne == 1)
                    {                        
                        CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                    }
                }
                if(CoupListe[IndexCurrentCoup].piece.info.type == TypePiece.Roi && CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Noire)
                {
                    if(CoupListe[IndexCurrentCoup].depart.ligne == 0 && CoupListe[IndexCurrentCoup].depart.colonne == 4)
                    {
                        int temp = 0;
                        int trouverPlusBasDansLaListe=0;
                        foreach(Coup cR in CoupListe)
                        {
                            temp++;
                            if(cR.piece == CoupListe[IndexCurrentCoup].piece)
                            {
                                if (temp < IndexCurrentCoup) trouverPlusBasDansLaListe = 1;
                            }
                        }
                        if(trouverPlusBasDansLaListe == 0)
                        {
                            CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                        }
                    }
                }
                if (CoupListe[IndexCurrentCoup].piece.info.type == TypePiece.Roi && CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Blanche)
                {
                    if (CoupListe[IndexCurrentCoup].depart.ligne == 7 && CoupListe[IndexCurrentCoup].depart.colonne == 4)
                    {
                        int temp = 0;
                        int trouverPlusBasDansLaListe = 0;
                        foreach (Coup cR in CoupListe)
                        {
                            temp++;
                            if (cR.piece == CoupListe[IndexCurrentCoup].piece)
                            {
                                if (temp < IndexCurrentCoup) trouverPlusBasDansLaListe = 1;
                            }
                        }
                        if (trouverPlusBasDansLaListe == 0)
                        {
                            CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                        }
                    }
                }

                if (CoupListe[IndexCurrentCoup].piece.info.type == TypePiece.Tour && CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Noire)
                {
                    if (CoupListe[IndexCurrentCoup].depart.ligne == 0 && CoupListe[IndexCurrentCoup].depart.colonne == 0)
                    {
                        int temp = 0;
                        int trouverPlusBasDansLaListe = 0;
                        foreach (Coup cR in CoupListe)
                        {
                            temp++;
                            if (cR.piece == CoupListe[IndexCurrentCoup].piece)
                            {
                                if (temp < IndexCurrentCoup) trouverPlusBasDansLaListe = 1;
                            }
                        }
                        if (trouverPlusBasDansLaListe == 0)
                        {
                            CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                        }
                    }
                }
                if (CoupListe[IndexCurrentCoup].piece.info.type == TypePiece.Tour && CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Noire)
                {
                    if (CoupListe[IndexCurrentCoup].depart.ligne == 0 && CoupListe[IndexCurrentCoup].depart.colonne == 7)
                    {
                        int temp = 0;
                        int trouverPlusBasDansLaListe = 0;
                        foreach (Coup cR in CoupListe)
                        {
                            temp++;
                            if (cR.piece == CoupListe[IndexCurrentCoup].piece)
                            {
                                if (temp < IndexCurrentCoup) trouverPlusBasDansLaListe = 1;
                            }
                        }
                        if (trouverPlusBasDansLaListe == 0)
                        {
                            CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                        }
                    }
                }
                if (CoupListe[IndexCurrentCoup].piece.info.type == TypePiece.Tour && CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Blanche)
                {
                    if (CoupListe[IndexCurrentCoup].depart.ligne == 7 && CoupListe[IndexCurrentCoup].depart.colonne == 0)
                    {
                        int temp = 0;
                        int trouverPlusBasDansLaListe = 0;
                        foreach (Coup cR in CoupListe)
                        {
                            temp++;
                            if (cR.piece == CoupListe[IndexCurrentCoup].piece)
                            {
                                if (temp < IndexCurrentCoup) trouverPlusBasDansLaListe = 1;
                            }
                        }
                        if (trouverPlusBasDansLaListe == 0)
                        {
                            CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                        }
                    }
                }
                if (CoupListe[IndexCurrentCoup].piece.info.type == TypePiece.Tour && CoupListe[IndexCurrentCoup].piece.info.couleur == CouleurCamp.Blanche)
                {
                    if (CoupListe[IndexCurrentCoup].depart.ligne == 7 && CoupListe[IndexCurrentCoup].depart.colonne == 7)
                    {
                        int temp = 0;
                        int trouverPlusBasDansLaListe = 0;
                        foreach (Coup cR in CoupListe)
                        {
                            temp++;
                            if (cR.piece == CoupListe[IndexCurrentCoup].piece)
                            {
                                if (temp < IndexCurrentCoup) trouverPlusBasDansLaListe = 1;
                            }
                        }
                        if (trouverPlusBasDansLaListe == 0)
                        {
                            CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                        }
                    }
                }
                RestartPrévisualisation();               

                if (IndexCurrentCoup > 0)
                {
                    if (CoupListe[IndexCurrentCoup].RockL)
                    {
                        CoupListe[IndexCurrentCoup].piece.setFirstPosition();
                        CoupListe[IndexCurrentCoup - 1].piece.setFirstPosition();
                        IndexCurrentCoup--;
                        Undo();
                        return;
                    }
                }
                undo = true;
                IndexCurrentCoup--;
            }
            else
            {
                undo = true;
            }
        }

        public void Redo()
        {
            IndexCurrentCoup++;
            if (IndexCurrentCoup < CoupListe.Count() && IndexCurrentCoup >= 0)
            {
                CoupListe[IndexCurrentCoup].arriver.Link(CoupListe[IndexCurrentCoup].piece);
                //ChangerEtat();                
                StatusChange();
                if (CoupListe[IndexCurrentCoup].deadPiece != null)
                {
                    if (CoupListe[IndexCurrentCoup].priseEnPassante == true)
                    {
                        CoupListe[IndexCurrentCoup].deadPiece.position.UnLink(CoupListe[IndexCurrentCoup].deadPiece);
                    }

                    if (CoupListe[IndexCurrentCoup].deadPiece.info.couleur == CouleurCamp.Blanche)
                    {
                        blancs.deadPiece.Add(CoupListe[IndexCurrentCoup].deadPiece.info);
                        DeadPiece();
                    }
                    else
                    {
                        noirs.deadPiece.Add(CoupListe[IndexCurrentCoup].deadPiece.info);
                        DeadPiece();
                    }
                }

                if(CoupListe[IndexCurrentCoup].Rock)
                {                    
                    Redo();
                    return;
                }
                RestartPrévisualisation();               
            }
            else IndexCurrentCoup--;             
        }

        public void MooveCleanListCoup()
        {
            List<Coup> CoupToRemove = new List<Coup>();
            foreach(Coup c in CoupListe)
            {
                if(c.Index > IndexCurrentCoup)
                {
                    CoupToRemove.Add(c);
                }
            }
            foreach(Coup cR in CoupToRemove)
            {
                CoupListe.Remove(cR);
            }
        }

        public StatusPartie MiseEnEchec()
        {
            if (status == StatusPartie.TraitBlancs)
            {
                foreach (Piece p in noirs.pieces)
                {
                    if (p.info.type == TypePiece.Roi)
                    {
                        foreach (Case c in blancs.destinationAllPiece)
                        {
                            if (c == p.position)
                            {
                                return StatusPartie.EchecNoirs;
                            }
                        }
                    }
                }
            }
            else if (status == StatusPartie.TraitNoirs)
            {
                foreach (Piece p in blancs.pieces)
                {
                    if (p.info.type == TypePiece.Roi)
                    {
                        foreach (Case c in noirs.destinationAllPiece)
                        {
                            if (c == p.position)
                            {
                                return StatusPartie.EchecBlancs;
                            }
                        }
                    }
                }
            }
            else
            {
                ChangerEtat();
                ChangeState = true;
            }
            return status;
        }

        public bool VerifieSiOnPeutMoove()
        {
            if (status.couleur == CouleurCamp.Blanche)
            {
                int tmp = 0;
                foreach (Piece p in blancs.pieces)
                {
                    if (p.listCaseDestination != null)
                    {
                        foreach (Case c in p.listCaseDestination)
                        {
                            if (p.Destination(c) == true)
                            {
                                tmp++;
                            }
                        }
                    }
                }
                if (tmp == 0) return false;
                else return true;
            }
            else
            {
                int tmp = 0;
                foreach (Piece p in noirs.pieces)
                {
                    if (p.listCaseDestination != null)
                    {
                        foreach (Case c in p.listCaseDestination)
                        {
                            if (p.Destination(c) == true)
                            {
                                tmp++;
                            }
                        }
                    }
                }
                if (tmp == 0) return false;
                else return true;
            }
        }

        public bool VerifiEviterMat()
        {
            if(status.couleur == CouleurCamp.Blanche)
            {
                foreach (Piece p in blancs.pieces)
                {
                    Case position = p.position;
                    
                    if (p.listCaseDestination != null)
                    {
                        foreach (Case c in p.listCaseDestination)
                        {
                            if (p.Destination(c) == true)
                            {
                                Piece ka = null;
                                if (c.piece != null)
                                {
                                    ka = c.piece;
                                }
                                c.Link(p);
                                StatusPartie Test = VerifiEnEchec();
                                position.Link(p);
                                if (ka != null) c.Link(ka);
                                MAJdestinationAllPiece();
                                if (Test.etat == EtatPartie.Nul)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Piece p in noirs.pieces)
                {
                    Case position = p.position;
                    if (p.listCaseDestination != null)
                    {
                        foreach (Case c in p.listCaseDestination)
                        {
                            if (p.Destination(c) == true)
                            {
                                Piece ka = null;
                                if (c.piece != null)
                                {
                                    ka = c.piece;
                                }
                                c.Link(p);
                                StatusPartie Test = VerifiEnEchec();
                                position.Link(p);
                                if (ka != null) c.Link(ka);
                                MAJdestinationAllPiece();
                                if (Test.etat == EtatPartie.Nul)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public StatusPartie VerifiEnEchec()
        {
            MAJdestinationAllPiece();
            if (status == StatusPartie.EchecNoirs)
            {
                foreach (Piece p in noirs.pieces)
                {
                    if (p.info.type == TypePiece.Roi)
                    {
                        foreach (Case c in blancs.destinationAllPiece)
                        {
                            if (c == p.position)
                            {
                                return StatusPartie.EchecNoirs;
                            }
                        }
                    }
                }
            }
            else if (status == StatusPartie.EchecBlancs)
            {
                foreach (Piece p in blancs.pieces)
                {
                    if (p.info.type == TypePiece.Roi)
                    {
                        foreach (Case c in noirs.destinationAllPiece)
                        {
                            if (c == p.position)
                            {
                                return StatusPartie.EchecBlancs;
                            }
                        }
                    }
                }
            }
            return StatusPartie.Nul;
        }

        public void MAJdestinationAllPiece()
        {
            RestartPrévisualisation();
            listCaseDestinationAllPiece();
        }

        public bool Plus50CoupNull()
        {
            int tmp = 0;
            foreach(Coup c in CoupListe)
            {
                if (c.deadPiece != null)
                {
                    tmp = 0;
                }
                else tmp++;
            }
            if(tmp >= 50)
            {
                return true;
            }
            return false;
        }

        public bool partieNull()
        {
            if (status.etat != EtatPartie.Echec)
            {
                if (status.couleur == CouleurCamp.Blanche)
                {
                    int tmp = 0;
                    foreach (Piece p in noirs.pieces)
                    {
                        if (p.listCaseDestination != null)
                        {
                            foreach (Case c in p.listCaseDestination)
                            {
                                if (p.Destination(c) == true)
                                {
                                    tmp++;
                                }
                            }
                        }
                    }
                    if (tmp == 0) return true;
                    else return false;
                }
                else
                {
                    int tmp = 0;
                    foreach (Piece p in blancs.pieces)
                    {
                        if (p.listCaseDestination != null)
                        {
                            foreach (Case c in p.listCaseDestination)
                            {
                                if (p.Destination(c) == true)
                                {
                                    tmp++;
                                }
                            }
                        }
                    }
                    if (tmp == 0) return true;
                    else return false;
                }
            }
            return false;
        }
    }
}
