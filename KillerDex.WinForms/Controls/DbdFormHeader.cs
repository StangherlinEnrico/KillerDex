using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KillerDex.Theme;

namespace KillerDex.Controls
{
    /// <summary>
    /// Custom header control for forms with Dead by Daylight styling.
    /// Displays a title, subtitle, optional actions panel, and accent line.
    /// </summary>
    public class DbdFormHeader : UserControl
    {
        #region Private Fields

        private Label _lblTitle;
        private Label _lblSubtitle;
        private Panel _pnlAccent;
        private Panel _pnlActions;

        private string _title = "";
        private string _subtitle = "";
        private float _titleFontSize = 20F;
        private bool _showAccentLine = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the title text.
        /// </summary>
        [Category("Appearance")]
        [Description("The main title text displayed in the header.")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                if (_lblTitle != null)
                {
                    _lblTitle.Text = value;
                    UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the subtitle text.
        /// </summary>
        [Category("Appearance")]
        [Description("The subtitle text displayed below the title.")]
        public string Subtitle
        {
            get => _subtitle;
            set
            {
                _subtitle = value;
                if (_lblSubtitle != null)
                {
                    _lblSubtitle.Text = value;
                    _lblSubtitle.Visible = !string.IsNullOrEmpty(value);
                    UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font size for the title.
        /// </summary>
        [Category("Appearance")]
        [Description("The font size for the title text.")]
        [DefaultValue(20F)]
        public float TitleFontSize
        {
            get => _titleFontSize;
            set
            {
                _titleFontSize = value;
                if (_lblTitle != null)
                {
                    _lblTitle.Font = new Font("Segoe UI", value, FontStyle.Bold);
                    UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to show the accent line at the bottom.
        /// </summary>
        [Category("Appearance")]
        [Description("Whether to display the red accent line at the bottom.")]
        [DefaultValue(true)]
        public bool ShowAccentLine
        {
            get => _showAccentLine;
            set
            {
                _showAccentLine = value;
                if (_pnlAccent != null)
                {
                    _pnlAccent.Visible = value;
                    UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Gets the actions panel where custom controls can be added.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel ActionsPanel => _pnlActions;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DbdFormHeader control.
        /// </summary>
        public DbdFormHeader()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            InitializeControls();
        }

        #endregion

        #region Initialization

        private void InitializeControls()
        {
            // Set control properties
            BackColor = DbdColors.HeaderBackground;
            Dock = DockStyle.Top;
            Height = 80;
            Padding = new Padding(25, 15, 25, 0);

            // Create title label
            _lblTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", _titleFontSize, FontStyle.Bold),
                ForeColor = DbdColors.TextPrimary,
                BackColor = Color.Transparent,
                Location = new Point(25, 15)
            };

            // Create subtitle label
            _lblSubtitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = DbdColors.TextAccent,
                BackColor = Color.Transparent,
                Visible = false
            };

            // Create actions panel (docked right)
            _pnlActions = new Panel
            {
                BackColor = Color.Transparent,
                Dock = DockStyle.Right,
                Width = 150,
                Padding = new Padding(0, 10, 0, 10)
            };

            // Create accent line
            _pnlAccent = new Panel
            {
                BackColor = DbdColors.AccentRed,
                Dock = DockStyle.Bottom,
                Height = 3
            };

            // Add controls in correct order
            Controls.Add(_lblSubtitle);
            Controls.Add(_lblTitle);
            Controls.Add(_pnlActions);
            Controls.Add(_pnlAccent);

            UpdateLayout();
        }

        #endregion

        #region Layout

        private void UpdateLayout()
        {
            if (_lblTitle == null || _lblSubtitle == null) return;

            // Calculate heights based on font metrics
            int titleHeight = GetTextHeight(_lblTitle.Font);
            int subtitleHeight = GetTextHeight(_lblSubtitle.Font);

            // Position title
            _lblTitle.Location = new Point(25, 15);

            // Position subtitle below title
            int subtitleY = 15 + titleHeight + 4;
            _lblSubtitle.Location = new Point(28, subtitleY);

            // Calculate required height
            int contentHeight = 15; // Top padding
            contentHeight += titleHeight;

            if (!string.IsNullOrEmpty(_subtitle) && _lblSubtitle.Visible)
            {
                contentHeight += 4 + subtitleHeight;
            }

            contentHeight += 12; // Bottom padding before accent

            if (_showAccentLine)
            {
                contentHeight += 3; // Accent line height
            }

            Height = Math.Max(contentHeight, 70);
        }

        private int GetTextHeight(Font font)
        {
            if (!IsHandleCreated)
            {
                // Estimate based on font size
                return (int)(font.Size * 1.6f);
            }

            using (var g = CreateGraphics())
            {
                return (int)Math.Ceiling(g.MeasureString("Wg", font).Height);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateLayout();
        }

        #endregion
    }
}
