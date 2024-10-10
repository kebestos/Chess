using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    public class Dame : Piece
    {
        public Dame(Joueur joueur) : base(joueur, TypePiece.Dame) { }

        public override bool Deplacer(Case destination)
        {
            if (destination.piece == null || destination.piece.info.couleur != this.info.couleur)
            {

                //Moove Top
                for (int i = 1; i < 8; i++)
                {
                    if (position.ligne - i > destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne, position.ligne - i].piece != null)
                        {
                            break;
                        }
                    }

                    if (destination.ligne == position.ligne - i && destination.colonne == position.colonne)
                    {
                        if (destination.piece != null)
                        {
                            destination.piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                            if (this.joueur.couleur == CouleurCamp.Blanche)
                            {
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else
                            {
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                        }
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        destination.Link(this);
                        StopPriseEnPassant();
                        return true;
                    }
                }

                //Moove Bottom
                for (int i = 1; i < 8; i++)
                {
                    if (position.ligne + i < destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne, position.ligne + i].piece != null)
                        {
                            break;
                        }
                    }

                    if (destination.ligne == position.ligne + i && destination.colonne == position.colonne)
                    {
                        if (destination.piece != null)
                        {
                            destination.piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                            if (this.joueur.couleur == CouleurCamp.Blanche)
                            {
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else
                            {
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                        }
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        destination.Link(this);
                        StopPriseEnPassant();
                        return true;
                    }
                }

                //MooveRight
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne - i > destination.colonne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - i, position.ligne].piece != null)
                        {
                            break;
                        }
                    }

                    if (destination.ligne == position.ligne && destination.colonne == position.colonne - i)
                    {                        
                        if (destination.piece != null)
                        {
                            destination.piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                            if (this.joueur.couleur == CouleurCamp.Blanche)
                            {
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else
                            {
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                        }
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        destination.Link(this);
                        StopPriseEnPassant();
                        return true;
                    }
                }

                //MooveLeft
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne + i < destination.colonne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + i, position.ligne].piece != null)
                        {
                            break;
                        }
                    }

                    if (destination.ligne == position.ligne && destination.colonne == position.colonne + i)
                    {
                       
                        if (destination.piece != null)
                        {
                            destination.piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                            if (this.joueur.couleur == CouleurCamp.Blanche)
                            {
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else
                            {
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                        }
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        destination.Link(this);
                        StopPriseEnPassant();
                        return true;
                    }
                }

                //MooveBottomRight
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne + i < destination.colonne && position.ligne + i < destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + i, position.ligne + i].piece != null)
                        {
                            break;
                        }
                    }
                    if ((destination.ligne == position.ligne + i && destination.colonne == position.colonne + i))
                    {
                        if (destination.piece != null)
                        {
                            destination.piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                            if (this.joueur.couleur == CouleurCamp.Blanche)
                            {
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else
                            {
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                        }
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        destination.Link(this);
                        StopPriseEnPassant();
                        return true;
                    }
                }
                //MooveBottomLeft
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne - i > destination.colonne && position.ligne + i < destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - i, position.ligne + i].piece != null)
                        {
                            break;
                        }
                    }
                    if ((destination.ligne == position.ligne + i && destination.colonne == position.colonne - i))
                    {
                        if (destination.piece != null)
                        {
                            destination.piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                            if (this.joueur.couleur == CouleurCamp.Blanche)
                            {
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else
                            {
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                        }
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        destination.Link(this);
                        StopPriseEnPassant();
                        return true;
                    }
                }

                //MooveTopRight
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne + i < destination.colonne && position.ligne - i > destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + i, position.ligne - i].piece != null)
                        {
                            break;
                        }
                    }

                    if ((destination.ligne == position.ligne - i && destination.colonne == position.colonne + i))
                    {
                        if (destination.piece != null)
                        {
                            destination.piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                            if (this.joueur.couleur == CouleurCamp.Blanche)
                            {
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else
                            {
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                        }
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        destination.Link(this);
                        StopPriseEnPassant();
                        return true;
                    }
                }

                //MooveTopLeft
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne - i > destination.colonne && position.ligne - i > destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - i, position.ligne - i].piece != null)
                        {
                            break;
                        }
                    }
                    if ((destination.ligne == position.ligne - i && destination.colonne == position.colonne - i))
                    {
                        if (destination.piece != null)
                        {
                            destination.piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                            if (this.joueur.couleur == CouleurCamp.Blanche)
                            {
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else
                            {
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                        }
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        destination.Link(this);
                        StopPriseEnPassant();
                        return true;
                    }
                }
            }
            return false;  
        }
        public override bool TourDeplacer()
        {
            return false;
        }
        public override bool PionPriseEnPassanr()
        {
            return false;
        }

        public override bool SetFalsePriseEnPassant()
        {
            return false;
        }

        public override bool Destination(Case destination)
        {
            if (destination.piece == null || destination.piece.info.couleur != this.info.couleur)
            {

                //Moove Top
                for (int i = 1; i < 8; i++)
                {
                    if(position.ligne -1 > 0)
                    {
                        if (position.ligne - i > destination.ligne)
                        {
                            if (joueur.partie.echiquier.cases[position.colonne, position.ligne - i].piece != null)
                            {
                                break;
                            }
                        }

                        if (destination.ligne == position.ligne - i && destination.colonne == position.colonne)
                        {
                            return true;
                        }
                    }                    
                }

                //Moove Bottom
                for (int i = 1; i < 8; i++)
                {
                    if (position.ligne + i < destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne, position.ligne + i].piece != null)
                        {
                            break;
                        }
                    }

                    if (destination.ligne == position.ligne + i && destination.colonne == position.colonne)
                    {                        
                        return true;
                    }
                }

                //MooveRight
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne - i > destination.colonne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - i, position.ligne].piece != null)
                        {
                            break;
                        }
                    }

                    if (destination.ligne == position.ligne && destination.colonne == position.colonne - i)
                    {
                        return true;
                    }
                }

                //MooveLeft
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne + i < destination.colonne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + i, position.ligne].piece != null)
                        {
                            break;
                        }
                    }

                    if (destination.ligne == position.ligne && destination.colonne == position.colonne + i)
                    {
                        return true;
                    }
                }

                //MooveBottomRight
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne + i < destination.colonne && position.ligne + i < destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + i, position.ligne + i].piece != null)
                        {
                            break;
                        }
                    }
                    if ((destination.ligne == position.ligne + i && destination.colonne == position.colonne + i))
                    {                        
                        return true;
                    }
                }
                //MooveBottomLeft
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne - i > destination.colonne && position.ligne + i < destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - i, position.ligne + i].piece != null)
                        {
                            break;
                        }
                    }
                    if ((destination.ligne == position.ligne + i && destination.colonne == position.colonne - i))
                    {
                        return true;
                    }
                }

                //MooveTopRight
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne + i < destination.colonne && position.ligne - i > destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + i, position.ligne - i].piece != null)
                        {
                            break;
                        }
                    }

                    if ((destination.ligne == position.ligne - i && destination.colonne == position.colonne + i))
                    {                        
                        return true;
                    }
                }

                //MooveTopLeft
                for (int i = 1; i < 8; i++)
                {
                    if (position.colonne - i > destination.colonne && position.ligne - i > destination.ligne)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - i, position.ligne - i].piece != null)
                        {
                            break;
                        }
                    }
                    if ((destination.ligne == position.ligne - i && destination.colonne == position.colonne - i))
                    {                        
                        return true;
                    }
                }
            }
            return false;
        }
        public override void setFirstPosition() { }
        public override bool SetRockTourTrue()
        {
            return false;
        }
        public override bool SetTruePriseEnPassant()
        {
            return false;
        }
    }
}
