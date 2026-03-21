namespace Сompiler
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            splitContainer1 = new SplitContainer();
            tabControlEditor = new TabControl();
            tabControlOutput = new TabControl();
            tabPage1 = new TabPage();
            dataGridParser = new DataGridView();
            Неверный = new DataGridViewTextBoxColumn();
            Местоположение = new DataGridViewTextBoxColumn();
            Описание = new DataGridViewTextBoxColumn();
            tabPage2 = new TabPage();
            gridScanner = new DataGridView();
            Code = new DataGridViewTextBoxColumn();
            Type = new DataGridViewTextBoxColumn();
            Lexeme = new DataGridViewTextBoxColumn();
            Position = new DataGridViewTextBoxColumn();
            menuStrip1 = new MenuStrip();
            файлToolStripMenuItem = new ToolStripMenuItem();
            создатьToolStripMenuItem = new ToolStripMenuItem();
            открытьToolStripMenuItem = new ToolStripMenuItem();
            сохранитьToolStripMenuItem = new ToolStripMenuItem();
            сохранитьКакToolStripMenuItem = new ToolStripMenuItem();
            выходToolStripMenuItem = new ToolStripMenuItem();
            правкаToolStripMenuItem = new ToolStripMenuItem();
            отменитьToolStripMenuItem = new ToolStripMenuItem();
            повторToolStripMenuItem = new ToolStripMenuItem();
            вырезатьToolStripMenuItem = new ToolStripMenuItem();
            копироватьToolStripMenuItem = new ToolStripMenuItem();
            вставитьToolStripMenuItem = new ToolStripMenuItem();
            удалитьToolStripMenuItem = new ToolStripMenuItem();
            выделитьВсеToolStripMenuItem = new ToolStripMenuItem();
            текстToolStripMenuItem = new ToolStripMenuItem();
            постановкаЗадачиToolStripMenuItem = new ToolStripMenuItem();
            грамматикаToolStripMenuItem = new ToolStripMenuItem();
            класификацияГрамматикиToolStripMenuItem = new ToolStripMenuItem();
            методАнализаToolStripMenuItem = new ToolStripMenuItem();
            тестовыйПримерToolStripMenuItem = new ToolStripMenuItem();
            списокЛитературыToolStripMenuItem = new ToolStripMenuItem();
            исходныйКодПргограммыToolStripMenuItem = new ToolStripMenuItem();
            пускToolStripMenuItem = new ToolStripMenuItem();
            парсерToolStripMenuItem = new ToolStripMenuItem();
            сканерToolStripMenuItem = new ToolStripMenuItem();
            справкаToolStripMenuItem = new ToolStripMenuItem();
            вызовСправкиToolStripMenuItem = new ToolStripMenuItem();
            оПрограммеToolStripMenuItem = new ToolStripMenuItem();
            локализацияToolStripMenuItem = new ToolStripMenuItem();
            russianToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            видToolStripMenuItem = new ToolStripMenuItem();
            размерШрифтаToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            toolStripButton4 = new ToolStripButton();
            toolStripButton5 = new ToolStripButton();
            toolStripButton6 = new ToolStripButton();
            toolStripButton7 = new ToolStripButton();
            toolStripButton8 = new ToolStripButton();
            toolStripButton9 = new ToolStripButton();
            toolStripButton10 = new ToolStripButton();
            toolStripButton11 = new ToolStripButton();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            statusStrip = new StatusStrip();
            statusFileName = new ToolStripStatusLabel();
            statusCursor = new ToolStripStatusLabel();
            statusLines = new ToolStripStatusLabel();
            statusSize = new ToolStripStatusLabel();
            statusLang = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControlOutput.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridParser).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridScanner).BeginInit();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(splitContainer1.Panel1, "splitContainer1.Panel1");
            splitContainer1.Panel1.Controls.Add(tabControlEditor);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(splitContainer1.Panel2, "splitContainer1.Panel2");
            splitContainer1.Panel2.Controls.Add(tabControlOutput);
            // 
            // tabControlEditor
            // 
            resources.ApplyResources(tabControlEditor, "tabControlEditor");
            tabControlEditor.AllowDrop = true;
            tabControlEditor.Name = "tabControlEditor";
            tabControlEditor.SelectedIndex = 0;
            // 
            // tabControlOutput
            // 
            resources.ApplyResources(tabControlOutput, "tabControlOutput");
            tabControlOutput.Controls.Add(tabPage1);
            tabControlOutput.Controls.Add(tabPage2);
            tabControlOutput.Name = "tabControlOutput";
            tabControlOutput.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Controls.Add(dataGridParser);
            tabPage1.Name = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridParser
            // 
            resources.ApplyResources(dataGridParser, "dataGridParser");
            dataGridParser.AllowUserToAddRows = false;
            dataGridParser.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridParser.Columns.AddRange(new DataGridViewColumn[] { Неверный, Местоположение, Описание });
            dataGridParser.Name = "dataGridParser";
            dataGridParser.RowHeadersVisible = false;
            // 
            // Неверный
            // 
            Неверный.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(Неверный, "Неверный");
            Неверный.Name = "Неверный";
            // 
            // Местоположение
            // 
            Местоположение.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(Местоположение, "Местоположение");
            Местоположение.Name = "Местоположение";
            // 
            // Описание
            // 
            Описание.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(Описание, "Описание");
            Описание.Name = "Описание";
            // 
            // tabPage2
            // 
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Controls.Add(gridScanner);
            tabPage2.Name = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridScanner
            // 
            resources.ApplyResources(gridScanner, "gridScanner");
            gridScanner.AllowUserToAddRows = false;
            gridScanner.AllowUserToDeleteRows = false;
            gridScanner.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridScanner.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridScanner.Columns.AddRange(new DataGridViewColumn[] { Code, Type, Lexeme, Position });
            gridScanner.Name = "gridScanner";
            gridScanner.ReadOnly = true;
            gridScanner.RowHeadersVisible = false;
            gridScanner.CellClick += gridScanner_CellClick;
            // 
            // Code
            // 
            resources.ApplyResources(Code, "Code");
            Code.Name = "Code";
            Code.ReadOnly = true;
            // 
            // Type
            // 
            resources.ApplyResources(Type, "Type");
            Type.Name = "Type";
            Type.ReadOnly = true;
            // 
            // Lexeme
            // 
            resources.ApplyResources(Lexeme, "Lexeme");
            Lexeme.Name = "Lexeme";
            Lexeme.ReadOnly = true;
            // 
            // Position
            // 
            resources.ApplyResources(Position, "Position");
            Position.Name = "Position";
            Position.ReadOnly = true;
            // 
            // menuStrip1
            // 
            resources.ApplyResources(menuStrip1, "menuStrip1");
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem, правкаToolStripMenuItem, текстToolStripMenuItem, пускToolStripMenuItem, справкаToolStripMenuItem, локализацияToolStripMenuItem, видToolStripMenuItem });
            menuStrip1.Name = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            resources.ApplyResources(файлToolStripMenuItem, "файлToolStripMenuItem");
            файлToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { создатьToolStripMenuItem, открытьToolStripMenuItem, сохранитьToolStripMenuItem, сохранитьКакToolStripMenuItem, выходToolStripMenuItem });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            // 
            // создатьToolStripMenuItem
            // 
            resources.ApplyResources(создатьToolStripMenuItem, "создатьToolStripMenuItem");
            создатьToolStripMenuItem.Name = "создатьToolStripMenuItem";
            // 
            // открытьToolStripMenuItem
            // 
            resources.ApplyResources(открытьToolStripMenuItem, "открытьToolStripMenuItem");
            открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            // 
            // сохранитьToolStripMenuItem
            // 
            resources.ApplyResources(сохранитьToolStripMenuItem, "сохранитьToolStripMenuItem");
            сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            // 
            // сохранитьКакToolStripMenuItem
            // 
            resources.ApplyResources(сохранитьКакToolStripMenuItem, "сохранитьКакToolStripMenuItem");
            сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            сохранитьКакToolStripMenuItem.Click += сохранитьКакToolStripMenuItem_Click;
            // 
            // выходToolStripMenuItem
            // 
            resources.ApplyResources(выходToolStripMenuItem, "выходToolStripMenuItem");
            выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            выходToolStripMenuItem.Click += выходToolStripMenuItem_Click;
            // 
            // правкаToolStripMenuItem
            // 
            resources.ApplyResources(правкаToolStripMenuItem, "правкаToolStripMenuItem");
            правкаToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { отменитьToolStripMenuItem, повторToolStripMenuItem, вырезатьToolStripMenuItem, копироватьToolStripMenuItem, вставитьToolStripMenuItem, удалитьToolStripMenuItem, выделитьВсеToolStripMenuItem });
            правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            // 
            // отменитьToolStripMenuItem
            // 
            resources.ApplyResources(отменитьToolStripMenuItem, "отменитьToolStripMenuItem");
            отменитьToolStripMenuItem.Name = "отменитьToolStripMenuItem";
            // 
            // повторToolStripMenuItem
            // 
            resources.ApplyResources(повторToolStripMenuItem, "повторToolStripMenuItem");
            повторToolStripMenuItem.Name = "повторToolStripMenuItem";
            // 
            // вырезатьToolStripMenuItem
            // 
            resources.ApplyResources(вырезатьToolStripMenuItem, "вырезатьToolStripMenuItem");
            вырезатьToolStripMenuItem.Name = "вырезатьToolStripMenuItem";
            // 
            // копироватьToolStripMenuItem
            // 
            resources.ApplyResources(копироватьToolStripMenuItem, "копироватьToolStripMenuItem");
            копироватьToolStripMenuItem.Name = "копироватьToolStripMenuItem";
            // 
            // вставитьToolStripMenuItem
            // 
            resources.ApplyResources(вставитьToolStripMenuItem, "вставитьToolStripMenuItem");
            вставитьToolStripMenuItem.Name = "вставитьToolStripMenuItem";
            // 
            // удалитьToolStripMenuItem
            // 
            resources.ApplyResources(удалитьToolStripMenuItem, "удалитьToolStripMenuItem");
            удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            удалитьToolStripMenuItem.Click += удалитьToolStripMenuItem_Click;
            // 
            // выделитьВсеToolStripMenuItem
            // 
            resources.ApplyResources(выделитьВсеToolStripMenuItem, "выделитьВсеToolStripMenuItem");
            выделитьВсеToolStripMenuItem.Name = "выделитьВсеToolStripMenuItem";
            выделитьВсеToolStripMenuItem.Click += выделитьВсеToolStripMenuItem_Click;
            // 
            // текстToolStripMenuItem
            // 
            resources.ApplyResources(текстToolStripMenuItem, "текстToolStripMenuItem");
            текстToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { постановкаЗадачиToolStripMenuItem, грамматикаToolStripMenuItem, класификацияГрамматикиToolStripMenuItem, методАнализаToolStripMenuItem, тестовыйПримерToolStripMenuItem, списокЛитературыToolStripMenuItem, исходныйКодПргограммыToolStripMenuItem });
            текстToolStripMenuItem.Name = "текстToolStripMenuItem";
            // 
            // постановкаЗадачиToolStripMenuItem
            // 
            resources.ApplyResources(постановкаЗадачиToolStripMenuItem, "постановкаЗадачиToolStripMenuItem");
            постановкаЗадачиToolStripMenuItem.Name = "постановкаЗадачиToolStripMenuItem";
            // 
            // грамматикаToolStripMenuItem
            // 
            resources.ApplyResources(грамматикаToolStripMenuItem, "грамматикаToolStripMenuItem");
            грамматикаToolStripMenuItem.Name = "грамматикаToolStripMenuItem";
            // 
            // класификацияГрамматикиToolStripMenuItem
            // 
            resources.ApplyResources(класификацияГрамматикиToolStripMenuItem, "класификацияГрамматикиToolStripMenuItem");
            класификацияГрамматикиToolStripMenuItem.Name = "класификацияГрамматикиToolStripMenuItem";
            // 
            // методАнализаToolStripMenuItem
            // 
            resources.ApplyResources(методАнализаToolStripMenuItem, "методАнализаToolStripMenuItem");
            методАнализаToolStripMenuItem.Name = "методАнализаToolStripMenuItem";
            // 
            // тестовыйПримерToolStripMenuItem
            // 
            resources.ApplyResources(тестовыйПримерToolStripMenuItem, "тестовыйПримерToolStripMenuItem");
            тестовыйПримерToolStripMenuItem.Name = "тестовыйПримерToolStripMenuItem";
            // 
            // списокЛитературыToolStripMenuItem
            // 
            resources.ApplyResources(списокЛитературыToolStripMenuItem, "списокЛитературыToolStripMenuItem");
            списокЛитературыToolStripMenuItem.Name = "списокЛитературыToolStripMenuItem";
            // 
            // исходныйКодПргограммыToolStripMenuItem
            // 
            resources.ApplyResources(исходныйКодПргограммыToolStripMenuItem, "исходныйКодПргограммыToolStripMenuItem");
            исходныйКодПргограммыToolStripMenuItem.Name = "исходныйКодПргограммыToolStripMenuItem";
            // 
            // пускToolStripMenuItem
            // 
            resources.ApplyResources(пускToolStripMenuItem, "пускToolStripMenuItem");
            пускToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { парсерToolStripMenuItem, сканерToolStripMenuItem });
            пускToolStripMenuItem.Name = "пускToolStripMenuItem";
            // 
            // парсерToolStripMenuItem
            // 
            resources.ApplyResources(парсерToolStripMenuItem, "парсерToolStripMenuItem");
            парсерToolStripMenuItem.Name = "парсерToolStripMenuItem";
            // 
            // сканерToolStripMenuItem
            // 
            resources.ApplyResources(сканерToolStripMenuItem, "сканерToolStripMenuItem");
            сканерToolStripMenuItem.Name = "сканерToolStripMenuItem";
            сканерToolStripMenuItem.Click += сканерToolStripMenuItem_Click;
            // 
            // справкаToolStripMenuItem
            // 
            resources.ApplyResources(справкаToolStripMenuItem, "справкаToolStripMenuItem");
            справкаToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { вызовСправкиToolStripMenuItem, оПрограммеToolStripMenuItem });
            справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            // 
            // вызовСправкиToolStripMenuItem
            // 
            resources.ApplyResources(вызовСправкиToolStripMenuItem, "вызовСправкиToolStripMenuItem");
            вызовСправкиToolStripMenuItem.Name = "вызовСправкиToolStripMenuItem";
            // 
            // оПрограммеToolStripMenuItem
            // 
            resources.ApplyResources(оПрограммеToolStripMenuItem, "оПрограммеToolStripMenuItem");
            оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            // 
            // локализацияToolStripMenuItem
            // 
            resources.ApplyResources(локализацияToolStripMenuItem, "локализацияToolStripMenuItem");
            локализацияToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { russianToolStripMenuItem, englishToolStripMenuItem });
            локализацияToolStripMenuItem.Name = "локализацияToolStripMenuItem";
            // 
            // russianToolStripMenuItem
            // 
            resources.ApplyResources(russianToolStripMenuItem, "russianToolStripMenuItem");
            russianToolStripMenuItem.Name = "russianToolStripMenuItem";
            russianToolStripMenuItem.Click += russianToolStripMenuItem_Click;
            // 
            // englishToolStripMenuItem
            // 
            resources.ApplyResources(englishToolStripMenuItem, "englishToolStripMenuItem");
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Click += englishToolStripMenuItem_Click;
            // 
            // видToolStripMenuItem
            // 
            resources.ApplyResources(видToolStripMenuItem, "видToolStripMenuItem");
            видToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { размерШрифтаToolStripMenuItem });
            видToolStripMenuItem.Name = "видToolStripMenuItem";
            // 
            // размерШрифтаToolStripMenuItem
            // 
            resources.ApplyResources(размерШрифтаToolStripMenuItem, "размерШрифтаToolStripMenuItem");
            размерШрифтаToolStripMenuItem.Name = "размерШрифтаToolStripMenuItem";
            размерШрифтаToolStripMenuItem.Click += размерШрифтаToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            resources.ApplyResources(toolStrip1, "toolStrip1");
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripButton2, toolStripButton3, toolStripButton4, toolStripButton5, toolStripButton6, toolStripButton7, toolStripButton8, toolStripButton9, toolStripButton10, toolStripButton11 });
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Stretch = true;
            // 
            // toolStripButton1
            // 
            resources.ApplyResources(toolStripButton1, "toolStripButton1");
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Name = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            resources.ApplyResources(toolStripButton2, "toolStripButton2");
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Name = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            resources.ApplyResources(toolStripButton3, "toolStripButton3");
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.Name = "toolStripButton3";
            // 
            // toolStripButton4
            // 
            resources.ApplyResources(toolStripButton4, "toolStripButton4");
            toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton4.Name = "toolStripButton4";
            // 
            // toolStripButton5
            // 
            resources.ApplyResources(toolStripButton5, "toolStripButton5");
            toolStripButton5.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton5.Name = "toolStripButton5";
            // 
            // toolStripButton6
            // 
            resources.ApplyResources(toolStripButton6, "toolStripButton6");
            toolStripButton6.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton6.Name = "toolStripButton6";
            // 
            // toolStripButton7
            // 
            resources.ApplyResources(toolStripButton7, "toolStripButton7");
            toolStripButton7.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton7.Name = "toolStripButton7";
            // 
            // toolStripButton8
            // 
            resources.ApplyResources(toolStripButton8, "toolStripButton8");
            toolStripButton8.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton8.Name = "toolStripButton8";
            // 
            // toolStripButton9
            // 
            resources.ApplyResources(toolStripButton9, "toolStripButton9");
            toolStripButton9.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton9.Name = "toolStripButton9";
            // 
            // toolStripButton10
            // 
            resources.ApplyResources(toolStripButton10, "toolStripButton10");
            toolStripButton10.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton10.Name = "toolStripButton10";
            // 
            // toolStripButton11
            // 
            resources.ApplyResources(toolStripButton11, "toolStripButton11");
            toolStripButton11.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton11.Name = "toolStripButton11";
            // 
            // statusStrip
            // 
            resources.ApplyResources(statusStrip, "statusStrip");
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusFileName, statusCursor, statusLines, statusSize, statusLang });
            statusStrip.Name = "statusStrip";
            // 
            // statusFileName
            // 
            resources.ApplyResources(statusFileName, "statusFileName");
            statusFileName.Name = "statusFileName";
            // 
            // statusCursor
            // 
            resources.ApplyResources(statusCursor, "statusCursor");
            statusCursor.Name = "statusCursor";
            // 
            // statusLines
            // 
            resources.ApplyResources(statusLines, "statusLines");
            statusLines.Name = "statusLines";
            // 
            // statusSize
            // 
            resources.ApplyResources(statusSize, "statusSize");
            statusSize.Name = "statusSize";
            // 
            // statusLang
            // 
            resources.ApplyResources(statusLang, "statusLang");
            statusLang.Name = "statusLang";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(statusStrip);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControlOutput.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridParser).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridScanner).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem файлToolStripMenuItem;
        private ToolStripMenuItem правкаToolStripMenuItem;
        private ToolStripMenuItem текстToolStripMenuItem;
        private ToolStripMenuItem пускToolStripMenuItem;
        private ToolStripMenuItem справкаToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private ToolStripButton toolStripButton5;
        private ToolStripButton toolStripButton6;
        private ToolStripButton toolStripButton7;
        private ToolStripButton toolStripButton8;
        private ToolStripMenuItem локализацияToolStripMenuItem;
        private ToolStripMenuItem видToolStripMenuItem;
        private ToolStripButton toolStripButton9;
        private ToolStripButton toolStripButton10;
        private ToolStripButton toolStripButton11;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem создатьToolStripMenuItem;
        private ToolStripMenuItem открытьToolStripMenuItem;
        private ToolStripMenuItem сохранитьToolStripMenuItem;
        private ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private ToolStripMenuItem выходToolStripMenuItem;
        private ToolStripMenuItem отменитьToolStripMenuItem;
        private ToolStripMenuItem повторToolStripMenuItem;
        private ToolStripMenuItem вырезатьToolStripMenuItem;
        private ToolStripMenuItem копироватьToolStripMenuItem;
        private ToolStripMenuItem вставитьToolStripMenuItem;
        private ToolStripMenuItem удалитьToolStripMenuItem;
        private ToolStripMenuItem выделитьВсеToolStripMenuItem;
        private ToolStripMenuItem постановкаЗадачиToolStripMenuItem;
        private ToolStripMenuItem грамматикаToolStripMenuItem;
        private ToolStripMenuItem класификацияГрамматикиToolStripMenuItem;
        private ToolStripMenuItem методАнализаToolStripMenuItem;
        private ToolStripMenuItem тестовыйПримерToolStripMenuItem;
        private ToolStripMenuItem списокЛитературыToolStripMenuItem;
        private ToolStripMenuItem исходныйКодПргограммыToolStripMenuItem;
        private ToolStripMenuItem вызовСправкиToolStripMenuItem;
        private ToolStripMenuItem оПрограммеToolStripMenuItem;
        private ToolStripMenuItem размерШрифтаToolStripMenuItem;
        private ToolStripMenuItem russianToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        internal TabControl tabControlEditor;
        private StatusStrip statusStrip;
        internal ToolStripStatusLabel statusFileName;
        internal ToolStripStatusLabel statusCursor;
        internal ToolStripStatusLabel statusSize;
        internal ToolStripStatusLabel statusLang;
        private SplitContainer splitContainer1;
        private TabControl tabControlOutput;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView gridScanner;
        internal ToolStripStatusLabel statusLines;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn Lexeme;
        private DataGridViewTextBoxColumn Position;
        private ToolStripMenuItem парсерToolStripMenuItem;
        private DataGridView dataGridParser;
        private DataGridViewTextBoxColumn Неверный;
        private DataGridViewTextBoxColumn Местоположение;
        private DataGridViewTextBoxColumn Описание;
        private ToolStripMenuItem сканерToolStripMenuItem;
    }
}
