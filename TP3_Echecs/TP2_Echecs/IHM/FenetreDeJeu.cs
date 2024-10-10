using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP2_Echecs.Echecs;

namespace TP2_Echecs.IHM
{
    public partial class FenetreDeJeu : Form, IEvenements
    {
        #region Attributs

        // référence sur la façade du << moteur de jeu >>
        IJeu jeu;

       

        // graphisme des carreaux de l'échiquier
        const int RANGEES        = 8;
        const int COLONNES       = 8;
        const int CARREAU_TAILLE = 42;
        Color     CARREAU_NOIR   = Color.FromArgb(189, 117, 53);
        Color     CARREAU_BLANC  = Color.FromArgb(229, 197, 105);

        // visualisation de l'échiquier
        PictureBox[,] carreaux = new PictureBox[COLONNES,RANGEES];

        // visualisation des captures
        const int CAPTURES = 15;
        PictureBox[] captures_blancs = new PictureBox[CAPTURES];
        PictureBox[] captures_noirs  = new PictureBox[CAPTURES];

        // liste des pieces
        List<Bitmap> piecesBlanches = new List<Bitmap>();
        List<Bitmap> piecesNoires   = new List<Bitmap>();

        // liste des curseurs
        Dictionary<InfoPiece, Cursor> curseurs = new Dictionary<InfoPiece, Cursor>();

        // géstion du drag & drop
        PictureBox picFrom, picTo;
        Image imgFrom;

        // status de la partie
        StatusPartie status;

        // chronomètres des jouers
        Stopwatch tempsBlancs = new Stopwatch();
        Stopwatch tempsNoirs  = new Stopwatch();

        //Historic Index
        int HistoricIndex = 0;


        #endregion

        #region Constructeur

        public FenetreDeJeu(IJeu jeu)
        {
            InitializeComponent();

            // initialisation de l'association
            this.jeu = jeu;
            this.jeu.vue = this;

            // initialisation de l'IHM
            CreationEchiquier();

            // initialisation de l'état
            status = StatusPartie.Reset;

            // commencer une nouvelle partie
            CommencerPartie();
        }

        #endregion

        #region Interface IEvenements

        public void ActualiserCase(int x, int y, InfoPiece info)
        {
            if (info == null)
                carreaux[x, y].Image = null;
            else if (info.couleur == CouleurCamp.Blanche)
                carreaux[x, y].Image = piecesBlanches[(int)info.type];           
            else
                carreaux[x, y].Image = piecesNoires[(int)info.type];
        }

        public void ActualiserCaptures(List<InfoPiece> pieces)
        {
            int idx_noirs = 0;
            int idx_blancs = 0;
            //Score
            int score_noir = 0;
            int score_blanc = 0;
            if(pieces != null)
            {
                foreach (InfoPiece p in pieces)
                {
                    if (p.couleur == CouleurCamp.Blanche)
                    {
                        captures_noirs[idx_noirs++].Image = piecesBlanches[(int)p.type];
                        if (p.type == TypePiece.Cavalier) score_noir += 3;
                        if (p.type == TypePiece.Dame) score_noir += 9;
                        if (p.type == TypePiece.Fou) score_noir += 3;
                        if (p.type == TypePiece.Tour) score_noir += 5;
                        if (p.type == TypePiece.Pion) score_noir += 1;
                    }
                    else
                    {
                        captures_blancs[idx_blancs++].Image = piecesNoires[(int)p.type];
                        if (p.type == TypePiece.Cavalier) score_blanc += 3;
                        if (p.type == TypePiece.Dame) score_blanc += 9;
                        if (p.type == TypePiece.Fou) score_blanc += 3;
                        if (p.type == TypePiece.Tour) score_blanc += 5;
                        if (p.type == TypePiece.Pion) score_blanc += 1;
                    }
                }
            }
            else
            {
                for (int i = 0; i < CAPTURES; i++)
                {
                    captures_noirs[i].Image = null;
                    captures_blancs[i].Image = null;                  
                    
                }
            }
            
            lblWhiteScore.Text = score_blanc.ToString();
            lblBlackScore.Text = score_noir.ToString();
        }

        public void ActualiserPartie(StatusPartie status)
        {
            this.status = status;

            // arreter les chronomètres
            tempsBlancs.Stop();
            tempsNoirs.Stop();

            // demarrer le chronomètre du joueur actif
            if (status.etat != EtatPartie.Reset && status.etat != EtatPartie.Mat)
            {
                if (status.couleur == CouleurCamp.Blanche)
                    tempsBlancs.Start();
                else
                    tempsNoirs.Start();
            }

            // actualiser les etiquettes des chronomètres
            RenderClockLabels(status);

            // demarrer/arreter le timer de l'IHM
            if (status.etat == EtatPartie.Reset || status.etat == EtatPartie.Mat || status.etat == EtatPartie.Nul)
                timer.Stop();
            else
            {
                if( !timer.Enabled )
                    timer.Start();
            }
           
        }

        #endregion

        #region Interface IJeu

        void CommencerPartie()
        {
            // reset des chronomètres
            tempsBlancs.Reset();
            tempsNoirs.Reset();

            // commencer une partie
            jeu.CommencerPartie();
        }

        void DeplacerPiece(int x_depart, int y_depart, int x_arrivee, int y_arrivee)
        {
            jeu.DeplacerPiece(x_depart, y_depart, x_arrivee, y_arrivee);
        }

        #endregion

        #region Fonctions de l'IHM

        void CreationEchiquier()
        {
            // création des carreaux pour l'échiquier
            int idx = 0;
            bool blanc;
            for (int y = 0; y < RANGEES; y++)
            {
                blanc = y % 2 == 0 ? true : false; 

                for (int x = 0; x < COLONNES; x++)
                {
                    carreaux[x, y] = new PictureBox();

                    carreaux[x, y].SizeMode = PictureBoxSizeMode.CenterImage;
                    carreaux[x, y].Size = new Size(CARREAU_TAILLE, CARREAU_TAILLE);
                    carreaux[x, y].Left = x * CARREAU_TAILLE + 2;
                    carreaux[x, y].Top  = y * CARREAU_TAILLE + 1;
                    carreaux[x, y].Tag = idx++;
                    carreaux[x, y].BackColor = blanc ? CARREAU_BLANC : CARREAU_NOIR;
                    blanc = !blanc;

                    carreaux[x, y].MouseDown    += carreau_MouseDown;
                    carreaux[x, y].DragEnter    += carreau_DragEnter;
                    carreaux[x, y].DragDrop     += carreau_DragDrop;
                    carreaux[x, y].GiveFeedback += carreau_GiveFeedback;
                    carreaux[x, y].AllowDrop = true;

                    pnlEdging.Controls.Add(carreaux[x, y]);
                }
            }

            // création des carreaux pour les piéces capturées
            for (int i = 0; i < CAPTURES; i++)
            {
                captures_blancs[i] = new PictureBox();
                captures_blancs[i].SizeMode = PictureBoxSizeMode.CenterImage;
                captures_blancs[i].Size = new Size(CARREAU_TAILLE, CARREAU_TAILLE);
                captures_blancs[i].Left = i * (CARREAU_TAILLE + 1) + 1;
                captures_blancs[i].Top = 384;
                captures_blancs[i].BackColor = SystemColors.ControlDark;
                captures_blancs[i].Tag = i;

                captures_noirs[i] = new PictureBox();
                captures_noirs[i].SizeMode = PictureBoxSizeMode.CenterImage;
                captures_noirs[i].Size = new Size(CARREAU_TAILLE, CARREAU_TAILLE);
                captures_noirs[i].Left = i * (CARREAU_TAILLE + 1) + 1;
                captures_noirs[i].Top = 384 + CARREAU_TAILLE + 1;
                captures_noirs[i].BackColor = SystemColors.ControlDark;
                captures_noirs[i].Tag = i;

                pnlMain.Controls.Add( captures_blancs[i] );
                pnlMain.Controls.Add( captures_noirs [i] );
            }

            // initialisation des images pour les pièces blanches
            piecesBlanches.Add(Properties.Resources.King_White);
            piecesBlanches.Add(Properties.Resources.Queen_White);
            piecesBlanches.Add(Properties.Resources.Rook_White);
            piecesBlanches.Add(Properties.Resources.Bishop_White);
            piecesBlanches.Add(Properties.Resources.Knight_White);
            piecesBlanches.Add(Properties.Resources.Pawn_White);

            // initialisation des images pour les pièces noires
            piecesNoires.Add(Properties.Resources.King_Black);
            piecesNoires.Add(Properties.Resources.Queen_Black);
            piecesNoires.Add(Properties.Resources.Rook_Black);
            piecesNoires.Add(Properties.Resources.Bishop_Black);
            piecesNoires.Add(Properties.Resources.Knight_Black);
            piecesNoires.Add(Properties.Resources.Pawn_Black);

            // associations images-pièces
            piecesBlanches[0].Tag = InfoPiece.RoiBlanc;
            piecesBlanches[1].Tag = InfoPiece.DameBlanche;
            piecesBlanches[2].Tag = InfoPiece.TourBlanche;
            piecesBlanches[3].Tag = InfoPiece.FouBlanc;
            piecesBlanches[4].Tag = InfoPiece.CavalierBlanc;
            piecesBlanches[5].Tag = InfoPiece.PionBlanc;
              piecesNoires[0].Tag = InfoPiece.RoiNoir;
              piecesNoires[1].Tag = InfoPiece.DameNoire;
              piecesNoires[2].Tag = InfoPiece.TourNoire;
              piecesNoires[3].Tag = InfoPiece.FouNoir;
              piecesNoires[4].Tag = InfoPiece.CavalierNoir;
              piecesNoires[5].Tag = InfoPiece.PionNoir;

            // création de la liste des curseurs 
            string strPath = "../../IHM/Cursors/";
            curseurs.Add( InfoPiece.RoiBlanc,      new Cursor(strPath + "WhiteKing.cur"));
            curseurs.Add( InfoPiece.DameBlanche,   new Cursor(strPath + "WhiteQueen.cur"));
            curseurs.Add( InfoPiece.TourBlanche,   new Cursor(strPath + "WhiteRook.cur"));
            curseurs.Add( InfoPiece.FouBlanc,      new Cursor(strPath + "WhiteBishop.cur"));
            curseurs.Add( InfoPiece.CavalierBlanc, new Cursor(strPath + "WhiteKnight.cur") );
            curseurs.Add( InfoPiece.PionBlanc,     new Cursor(strPath + "WhitePawn.cur"));
            curseurs.Add( InfoPiece.RoiNoir,       new Cursor(strPath + "BlackKing.cur"));
            curseurs.Add( InfoPiece.DameNoire,     new Cursor(strPath + "BlackQueen.cur"));
            curseurs.Add( InfoPiece.TourNoire,     new Cursor(strPath + "BlackRook.cur"));
            curseurs.Add( InfoPiece.FouNoir,       new Cursor(strPath + "BlackBishop.cur") );
            curseurs.Add( InfoPiece.CavalierNoir,  new Cursor(strPath + "BlackKnight.cur") );
            curseurs.Add( InfoPiece.PionNoir,      new Cursor(strPath + "BlackPawn.cur"));
        }

        void carreau_MouseDown(object sender, MouseEventArgs e)
        {
            // sauvegarder le carreau de départ
            picFrom = sender as PictureBox;
            imgFrom = picFrom.Image;

            // terminer s'il n'y a pas de pièce sur le carreau
            if (imgFrom == null)
                return;

            // informations sur la pièce contenue dans le carreau
            InfoPiece piece = imgFrom.Tag as InfoPiece;

            // terminer si la partie n'est pas active ou si la couleur de la piece selectionnée ne corresponde pas à la couleur du joueur qui joue dans ce tours
            if (status.etat == EtatPartie.Reset || status.etat == EtatPartie.Mat || status.couleur != piece.couleur || status.etat == EtatPartie.Nul)
            {
                //restartColorEchiquier();
                
                return;
            }               

            // demarrer le Drag & Drop
            picFrom.DoDragDrop(imgFrom, DragDropEffects.Move);

            // remettre le curseur
            pnlEdging.Cursor = Cursors.Default;

            // remettre l'image sur le carreau de départ
            picFrom.Image = imgFrom;
            picFrom.BorderStyle = BorderStyle.None;

            // terminer s'il n'y a pas de carreau cible
            if (picTo == null)
            {
                restartColorEchiquier();
                return;
            }               

            // calculer les indices des carreaux de départ et arrivée
            int idxFrom = Convert.ToInt32(picFrom.Tag);
            int idxTo = Convert.ToInt32(picTo.Tag);

            // reset du carreau cible
            picTo = null;

            // transformer les indices lineaires en numeros de rangée et colonne
            int x1 = idxFrom % 8;        // colonne du carreau de départ
            int y1 = (idxFrom - x1) / 8; // rangée du carreau de départ
            int x2 = idxTo % 8;          // colonne du carreau d'arrivée
            int y2 = (idxTo - x2) / 8;   // rangée du carreau d'arrivée

            // invoquer l'operation << DeplacerPiece >>
            DeplacerPiece(x1, y1, x2, y2);
            restartColorEchiquier();            
        }

        void carreau_DragDrop(object sender, DragEventArgs e)
        {
            // sauvegarder le carreau cible
            picTo = sender as PictureBox;
        }

        void carreau_DragEnter(object sender, DragEventArgs e)
        {
            // autoriser le drop                
            e.Effect = DragDropEffects.Move;

            // changer le curseur
            pnlEdging.Cursor = curseurs[imgFrom.Tag as InfoPiece];
            

            // effacer l'image du carreau de départ
            picFrom.Image = null;
            picFrom.Refresh();
            

            // marquer le carreau de départ
            picFrom.BorderStyle = BorderStyle.Fixed3D;

            //Prévisualisation 
            int idxFrom = Convert.ToInt32(picFrom.Tag);
            int x1 = idxFrom % 8;        // colonne du carreau de départ
            int y1 = (idxFrom - x1) / 8; // rangée du carreau de départ
            ActualiserDeplacement(jeu.GetCase(x1, y1));            

        }

        void carreau_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
        }

        void RenderClocks()
        {
            lblWhiteClock.Text = SpanToString(tempsBlancs.Elapsed);
            lblBlackClock.Text = SpanToString(tempsNoirs.Elapsed);
            lblWhiteClock.Refresh();
            lblBlackClock.Refresh();
        }

        void RenderClockLabels(StatusPartie status)
        {
            // selectionner l'etiquette du joueur actif
            Label lab1, lab2;
            if (status.couleur == CouleurCamp.Blanche)
            {
                lab1 = lblWhiteClock;
                lab2 = lblBlackClock;
            }
            else
            {
                lab1 = lblBlackClock;
                lab2 = lblWhiteClock;
            }
            
            // souligner le chronomètre du joueur actif
            if(status.etat != EtatPartie.Nul)
            {
                lab1.BorderStyle = BorderStyle.FixedSingle;
                lab2.BorderStyle = BorderStyle.None;
                lab1.BackColor = status.etat == EtatPartie.Mat ? Color.Red : (status.etat == EtatPartie.Echec ? Color.Orange : Color.LightGray);
                lab2.BackColor = Color.FromName(KnownColor.Control.ToString());
            }
            else
            {
                lab1.BorderStyle = BorderStyle.FixedSingle;
                lab2.BorderStyle = BorderStyle.None;
                lab1.BackColor = Color.Green;
                lab2.BackColor = Color.Green;
            }
            
        }

        string SpanToString(TimeSpan span)
        {
            return span.Hours.ToString().PadLeft(2, '0') 
                   + ":"
                   + span.Minutes.ToString().PadLeft(2, '0')
                   + ":"
                   + span.Seconds.ToString().PadLeft(2, '0');
        }

        private void tbr_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            switch (e.Button.Tag.ToString())
            {
                case "New":
                    CommencerPartie();
                    break;

                case "Open":
                    // TODO
                    break;

                case "Save":
                    // TODO
                    break;

                case "UndoMove":
                    jeu.Undo();
                    break;

                case "RedoMove":
                    jeu.Redo();
                    break;

                case "UndoAllMoves":
                    // TODO
                    break;

                case "RedoAllMoves":
                    // TODO
                    break;

                case "FlipBoard":
                    // TODO
                    break;

                case "Think":
                    // TODO
                    break;

                case "MoveNow":
                    // TODO
                    break;

                case "ResumePlay":
                    // TODO
                    break;

                case "PausePlay":
                    // TODO
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            RenderClocks();
        }

        #endregion

        public void ActualiserHistoric(Coup coup)
        {            
            if(coup != null)
            {
                String time;
                ListViewItem Element = new ListViewItem(HistoricIndex.ToString());
                if (coup.piece.info.couleur == CouleurCamp.Blanche)
                {
                    time = lblWhiteClock.Text;
                    Element.SubItems.Add(time);
                }
                else
                {
                    time = lblBlackClock.Text;
                    Element.SubItems.Add(time);
                }
                Element.SubItems.Add(coup.coup);
                lvwMoveHistory.Items.Add(Element);
                HistoricIndex += 1;
            }
            else
            {
                foreach(ListViewItem lvt in lvwMoveHistory.Items)
                {
                    lvwMoveHistory.Items.Remove(lvt);
                }
                HistoricIndex = 0;
            }
        }

        public void ActualiserDeplacement(Case cas)
        {            
            List<Case> cDestiny =  cas.piece.listCaseDestination;
            foreach(Case c in cDestiny)
            {
                carreaux[c.colonne, c.ligne].BackColor = Color.FromArgb(0, 117, 0);                
            }
        }

        public void restartColorEchiquier()
        {
            bool blanc;
            for (int y = 0; y < RANGEES; y++)
            {
                blanc = y % 2 == 0 ? true : false;

                for (int x = 0; x < COLONNES; x++)
                {                   
                    carreaux[x, y].BackColor = blanc ? CARREAU_BLANC : CARREAU_NOIR;
                    blanc = !blanc;
                }
            }
        }        
        
    }
}