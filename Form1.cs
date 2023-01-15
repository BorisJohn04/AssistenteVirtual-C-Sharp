using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Microsoft.Speech.Recognition;
using System.Diagnostics;
using System.Management;
using System.Net;

using System.Globalization;//informação de idioma de entrada
using System.Timers;
namespace VandaAssistant
{
    public partial class Form1 : Form
    {
        public static Button modoAdm;
        public static Button notSelect;
        public static bool admMode;
        public static int contadorAbas;
        public static int b; 
        public static int z;
        public static bool time2Reset;
        public static string name;
        public static int result = 0;
        public static int result2 = 0;
        public static double result3 = 0;
        public static string memory;
        public static int size = 0;
        public static int size2 = 0;
        public static int size3 = 0;
        public static int size34= 0;
        public static int count = 0;
        public static PerformanceCounter PC;
        public static int i = 0;
        //mensagens da legenda
        public static string legendaText;
        public static string[] nomes;
        public static bool apresentacao;
        public static int fadingSpeed = 0;
        public static int fadeCount;
        public static bool mostrouHora;
        public static int timeCount;
        public static string frasesIniciais;
        public static string comandoInicial;
        public static bool inSystem;
        public static bool iniciar;
        public static ListView lista;

        public static bool onRelogio;
        public static Panel relogioForm;
        public static Label hora;
        public static Label minuto;
        public static Label segundo;
        public static System.Timers.Timer tempo;
        public static System.Timers.Timer tempo2;
        public static System.Timers.Timer tempo3;
        public static double clock;
        public static int contadorList;
        public static bool listaCriada;
        public static bool otherGramatic;
        public static Label label1_Click;
        static int contador;
        static bool naoAchou;
        static bool fechouWordPad;
        static bool wordpadAtivo;
        static bool autoTypeAtivo;
        public static string palavraDita;
        public static bool wordPadCondition;
        public static bool OptionsSelect;
        public static CultureInfo idioma = new CultureInfo("pt-BR");//especifica o idioma do reconhecedor
        static SpeechSynthesizer reconhecedor = new SpeechSynthesizer();//inicia o sintetisador para reproduzir voz
        public static SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();//inicia a engine de recognição
        public static string palavra;//guarda a palavra que você falar para verificação
       
        public static bool autowrite;

        public class variaveis
        {

            public static Label message;

        }
        public Form1()
        {
            InitializeComponent();
            OptionsSelect = false;
            metodos.init();//chama a função de reconhecimento 
            // label1.Parent = relogioForm;
            timer1.Start();
            iniciar = false;
            mysqlConnection.connectionData();
            //tempo de checagem 
            tempo2 = new System.Timers.Timer();
            tempo2.Interval = 3000;
            tempo2.Elapsed += pro2;
            tempo2.AutoReset = true;
            //tempo para encerrar programa
            tempo3 = new System.Timers.Timer();
            tempo3.Interval = 2000;
            tempo3.Elapsed += encerramento;
            tempo3.AutoReset = true;


        }

        //private void MainForm_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Control && e.KeyCode == Keys.S)
        //    {
        //        reconhecedor.SpeakAsync("funcionou");
        //    }
        //}

        public void encerramento(object sender, ElapsedEventArgs e)
        {
            z += 1;
            if (z == 1)
            {
                foreach (var process in Process.GetProcessesByName("VandaAssistant"))
                {
                    process.Kill();
                }
                tempo3.Stop();
            }
        }
        public void pro2(object sender, ElapsedEventArgs e)//zera os somadores de memória, refaz a checagem, pausa o tempo2 e envia o resultado para interface
        {
            size = 0;
            size2 = 0;
            result = 0;
            result2 = 0;
            result3 = 0;
            foreach (var process in Process.GetProcesses())
            {
                //converter bytes em MB, basta dividir o numero de bytes por 1024 duas vezes, isso serve para todas as outras unidades, gigas, megas.
                PC = new PerformanceCounter();
                PC.CategoryName = "Process";
                PC.CounterName = "Working Set - Private";
                PC.InstanceName = process.ProcessName;
                size = Convert.ToInt32(PC.NextValue()) / (int)(1024);
                size2 += size;
                PC.Close();
                PC.Dispose();
            }//tempo de checagem
            tempo2.Stop();
            result = size2 / 100;
            result2 = result / 100;
            result3 = result2 / 10;

            legendaText = Math.Round(result3).ToString() + "% da RAM em uso".ToString();//e da um dignóstico
            if (result3 < 50)
            {
                reconhecedor.SpeakAsync("O consúmo de memória da máquina está baixo");
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
            }
            else if (result3 > 40 && result2 < 70) 
            {
                reconhecedor.SpeakAsync("O consúmo de memória da máquina está mediano");
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
               
            }
            else if (result3 > 60)
            {
                reconhecedor.SpeakAsync("O consúmo de memória da máquina está alto");
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
               
            }
            else if (result3 > 90)
            {
                reconhecedor.SpeakAsync("O consúmo de memória da máquina está muito alto");
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
               
            }
            CodigosVanda.legenda.Invoke(new Action(()=> CodigosVanda.legenda.Location = new Point(125, 310)));
            CodigosVanda.legenda.Invoke(new Action(()=> CodigosVanda.legenda.Text = legendaText.ToString()));
        }//essa função pode ser ativada quando você quiser consultar o uso de memória, ela checa a memória e ativa o tempo de mostragem.
    
        public class metodos: CodigosVanda
        {
          
                public static void gramatica()
                {

                    recognizer = new SpeechRecognitionEngine(idioma);//especifica o idioma de reconhecimento para o reconhecedor
                    var gramatic = new Choices();//cria novas escolhas
                    gramatic.Add(comandos2.words);//atribui as escolhas apenas para aquilo que tem na array de palavras

                    var gramaticaList = new GrammarBuilder();//fixa uma lista de escolhas para gramática criada
                    gramaticaList.Append(gramatic);//no caso, gramática que engloba words, sua array de palavras
                    var dialogo = new Grammar(gramaticaList);//inicia o grammabuilder

                    recognizer.RequestRecognizerUpdate();//atualiza o reconhecedor para novas entradas
                    recognizer.LoadGrammarAsync(dialogo);//carrega a gramática
                    recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recog.recognizer_SpeechRecognized);//chama função de recognição
                    recognizer.SetInputToDefaultAudioDevice();//seleciona o dispositivo de fala padrão usado pelo sistema
                    reconhecedor.SetOutputToDefaultAudioDevice();//seleciona o dispositivo de saida
                    recognizer.RecognizeAsync(RecognizeMode.Multiple);//especifica multiplos reconhecimentos, invés de apenas 1;



                }
            

        public static void init()
            {
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 3;
                gramatica(); 
            }         
        private void Form1_Load(object sender, EventArgs e)
            {

            }
       
            
        }//metodos de reconhecimento de voz

        public class recog: CodigosVanda
        {
            public static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)//função de reconhecimento
            {

                //comandos de inicialização
                
                //ATIVAÇÃO POR VOZ <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                if (palavraDita == "vanda, acorda" && iniciar == false)
                {
                    reconhecedor.Volume = 100;
                    reconhecedor.Rate = 2;
                    reconhecedor.SpeakAsync("Olá, desculpa a demora, estava em um sono profundo, eu sou a Vanda, uma assistente virtual prestativa e amigável, como devo te chamar?");
                    CodigosVanda.tela1();
                    CodigosVanda.tempo.Enabled = true;
                    
                    iniciar = true;

                }
                else if (palavraDita == "iniciar" && iniciar == false){
                    reconhecedor.Volume = 100;
                    reconhecedor.Rate = 2;
                    reconhecedor.SpeakAsync("Olá, eu sou a vanda, uma assistente virtual criada por um cara que acha que vai ficar rico programando");
                    CodigosVanda.tela1();
                    CodigosVanda.tempo.Enabled = true;
                    
                    iniciar = true;

                }else if(palavraDita == "inicia" && iniciar == false){
                    reconhecedor.Volume = 100;
                    reconhecedor.Rate = 2;
                    reconhecedor.SpeakAsync("Olá, eu sou a vanda, uma assistente virtual criada por um cara que acha que vai ficar rico programando");
                    CodigosVanda.tela1();
                    CodigosVanda.tempo.Enabled = true;
                    iniciar = true;
                }else if(palavraDita == "iniciá" && iniciar == false)
                {
                    reconhecedor.Volume = 100;
                    reconhecedor.Rate = 2;
                    reconhecedor.SpeakAsync("Olá, eu sou a vanda, uma assistente virtual criada por um cara que acha que vai ficar rico programando");
                    CodigosVanda.tela1();
                    CodigosVanda.tempo.Enabled = true;
 
                    iniciar = true;
                }


                // Modo padrão para usuários comuns
                if (iniciar == true)
                {
                    if(autoTypeAtivo == true)//enquanto a tivo, executa o allgramatics
                    {
                        //recognizer.LoadGrammarAsync(new DictationGrammar());
                        palavraDita = e.ToString();//salva a palavra dita nesta variável
                        palavraDita = e.ToString();
                        SendKeys.Send(e.Result.Text);
                        SendKeys.SendWait(" ");
                    }

                    //guarda a palavra captada pelo reconhecedor
                    palavraDita = e.Result.Text;//salva a palavra dita nesta variável

                    CodigosVanda.operacoes();//contém todas as operações aritiméticas
                    if(palavraDita == "vanda, prioridade máxima")
                    {
                        priorityGames();
                    }
                    
                    //if (palavraDita == "bái bái")
                   // {
                    //    reconhecedor.SpeakAsync("Até mais senhor");
                     //   tempo3.Start();
                    //}

                    //comandos de assistencia para cegos
                    if(palavraDita == "pesquisa")
                    {
                        palavraDita = "";
                        SendKeys.Send("{ENTER}");
                    }

                    if(palavraDita == "abre uma ába")
                    {
                        contadorAbas = 0;
                        SendKeys.Send(Keys.Control.ToString());
                        SendKeys.Send(Keys.T.ToString());

                        contadorAbas++;
               
                            if(contadorAbas == 1){
                            reconhecedor.SpeakAsync("uma ába foi criada");
                            }else if(contadorAbas > 1){
                                reconhecedor.SpeakAsync("mais uma ába foi criada");
                            }
                    }

                    if (palavraDita == "fecha uma ába")
                    {
                        contadorAbas--;
                        if (contadorAbas > 1)
                        {

                            reconhecedor.SpeakAsync("uma ába foi encerrada");
                        }
                        else if(contadorAbas < 2 && contadorAbas > 0){
                            foreach(var process in Process.GetProcessesByName("msedge"))
                            {
                                process.Kill();
                            }
                        }
                    }

                    if(palavraDita == "navegador interno")
                    {
                        reconhecedor.SpeakAsync("Navegador Rórus foi aberto");
                        createNavigator();
                    }

                    //------------------------------------------------------------
                    if (palavraDita == "abre o navegador")
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = "msedge.exe";
                        process.StartInfo.Arguments = "-n";
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        process.Start();
                        //Process.Start("Rundll32.exe", "shell32.dll, appwiz.cpl");//inicia um aplicativo de configurações do windows, por meio do Rundll32
                        reconhecedor.SpeakAsync("Navegador aberto senhor");

                    }

                    if (palavraDita == "uso de memória")
                    {
                        reconhecedor.SpeakAsync("Memória atualizada");
                        reconhecedor.Volume = 100;
                        reconhecedor.Rate = 4;
                        tempo2.Start();
                    }

                    if (palavraDita == "habilita escrita inteligente")
                    {
                        reconhecedor.SpeakAsync("O modo foi ativádo senhor");
                        autoTypeAtivo = true;

                    }

                    if (palavraDita == "vanda, tá ai?")
                    {
                        reconhecedor.SpeakAsync("Sim senhor, o que deseja?");
                    }

                    if (palavraDita == "fechar wordpad")
                    {

                        foreach (var process in Process.GetProcessesByName("wordpad"))
                        {
                            contador += 1;
                            if (process.ProcessName == "wordpad")
                            {
                                if (contador < 2)
                                {
                                    reconhecedor.SpeakAsync("processo localizado senhor, mas o senhor não pode encerrá-lo, se fizer isso, pode perder dados do seus sistema");
                                    wordPadCondition = true;

                                }
                                else if (contador > 1)
                                {
                                    reconhecedor.SpeakAsync("Senhor, há mais de duas instâncias do mesmo programa aberto");
                                    wordPadCondition = true;
                                }

                            }
                            else if (process.ProcessName != "wordpad")
                            {
                                naoAchou = true;
                            }

                        }

                        if (naoAchou == true)
                        {
                            reconhecedor.SpeakAsync("Senhor, esse programa ainda não foi iniciado");
                            naoAchou = false;
                        }
                    }

                    if (palavraDita == "vanda")
                    {
                        reconhecedor.SpeakAsync("Pois não?");
                    }

                    if (palavraDita == "abre o wordpad")
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = "wordpad.exe";
                        process.StartInfo.Arguments = "-n";
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        process.Start();
                        //Process.Start("Rundll32.exe", "shell32.dll, appwiz.cpl");//inicia um aplicativo de configurações do windows, por meio do Rundll32
                        reconhecedor.SpeakAsync("Programa iniciado senhor, o que deseja fazer?");
                        wordpadAtivo = true;
                    }

                    if (palavraDita == "vanda, inicia warcraft três")
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = "F:\\Warcraft III\\Frozen Throne.exe";
                        process.StartInfo.Arguments = "-n";
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        process.Start();
                        reconhecedor.SpeakAsync("É um ótimo jogo senhor, não sabia que gostava de jogá-lo");

                    }

                    if (palavraDita == "encerrar warcraft três")
                    {
                        foreach (var process in Process.GetProcessesByName("war3"))
                        {
                            process.Kill();
                        }
                        reconhecedor.SpeakAsync("Jogo encerrado. Espero que tenha ganho a partida senhor, se perdeu, faz parte");
                    }

                    if (palavraDita == "pode tirar um dia de folga")
                    {
                        reconhecedor.SpeakAsync("Estava precisando senhor, obrigado");
                    }

                    if (palavraDita == "fecha o navegador também")
                    {
                        foreach (var process in Process.GetProcessesByName("msedge"))
                        {
                            process.Kill();
                        }
                        reconhecedor.SpeakAsync("navegador encerrado senhor, deseja mais alguma coisa?");
                    }

                    if (palavraDita == "vanda, inicia o")
                    {
                        reconhecedor.SpeakAsync("iniciar o que senhor?");
                    }

                    if (palavraDita == "cria uma lista de processos")
                    {
                        interfaceConstruct();
                        reconhecedor.SpeakAsync("Lista criada senhor");
                        reconhecedor.SpeakAsync("Há um total de:'" + count + "'");
                        listaCriada = true;
                    }
                    //condições de programas
                    if (palavraDita == "sim" && wordPadCondition == true)
                    {
                        foreach (var process in Process.GetProcessesByName("wordpad"))
                        {
                            process.Kill();
                        }
                        wordpadAtivo = false;
                        autoTypeAtivo = false;
                        reconhecedor.SpeakAsync("O programa foi encerrado senhor");
                        contador = 0;
                        wordPadCondition = false;
                    }

                    if (listaCriada == true && palavraDita == "encerrar processo wordpad")
                    {
                        //int count2 = 0;

                        foreach (var process in Process.GetProcessesByName("wordpad"))
                        {


                            //CodigosVanda.interfaceLista.Items[count2].Selected = true;Seleciona o processo na lista e o marca

                            //CodigosVanda.interfaceLista.Items[count2].EnsureVisible();

                            process.Kill();
                            reconhecedor.SpeakAsync("processo encerrado");

                        }

                        foreach (string process2 in CodigosVanda.tasks)
                        {
                            contadorList += 1;
                            if (process2 == "wordpad")
                            {
                                CodigosVanda.tasks.Remove(process2);
                                CodigosVanda.interfaceLista.Items[contadorList].Remove();
                                CodigosVanda.interfaceLista.FindItemWithText(process2).Selected = true;
                                listaCriada = false;

                            }
                        }
                    }

                    if (palavraDita == "mostra a lista de comandos")
                    {
                        listaComandosInterface();
                        reconhecedor.SpeakAsync("Estes são os meus comandos operacionais");
                    }
                    else if (palavraDita == "comandos")
                    {
                        listaComandosInterface();
                        reconhecedor.SpeakAsync("Estes são os meus comandos operacionais");
                    }
                    else if (palavraDita == "lista de comandos")
                    {
                        listaComandosInterface();
                        reconhecedor.SpeakAsync("Estes são os meus comandos operacionais");
                    }

                    if (palavraDita == "que horas são")
                    {
                        onRelogio = true;
                        onClock();
                       
                    }

                    if (palavraDita == "desativa o relógio")
                    {
                        offClock();
                        onRelogio = false;
                        reconhecedor.SpeakAsync("ok");
                    }

                    if (palavraDita == "aumenta o tamanho do relógio" && onRelogio == true)
                    {
                        relogioForm.Size = new Size(1300, 380);
                        hora.Font = new Font("Microsoft Sans Serif; 8,25pt", 200);
                        hora.Size = new System.Drawing.Size(1300, 1100);
                        reconhecedor.SpeakAsync("Ta ficando cégo héim");
                    }

                    if (palavraDita == "atualiza a lista")
                    {
                        CodigosVanda.upgrade();
                    }
                    else if (palavraDita == "atualizar constantemente")
                    {
                        CodigosVanda.upgrade();
                    }
                }

                //Modo administrativo
                if(admMode == true)
                {
                   reconhecedor.SpeakAsync("Tudo certo, vamos trabalhar no modo administratívo");
                }     
            }
        }//função de recognição

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }
        public static void onClock()
        {

                //criando form do relogio
                relogioForm = new Panel();
                relogioForm.Size = new Size(160, 40);
                relogioForm.Location = new Point(1, 0);
                relogioForm.Visible = true;
                relogioForm.Text = "Relógio";
                relogioForm.BackColor = Color.Black;
                relogioForm.Parent = CodigosVanda.faceVandaMacro;
                relogioForm.BringToFront();
                relogioForm.Location = new Point(5, 400);
                //time do painel do relogio

                hora = new Label();
                hora.AutoSize = false;
                hora.Font = new Font("Microsoft Sans Serif; 8,25pt", 20);
                hora.Size = new System.Drawing.Size(180, 40);
                hora.Location = new Point(10, 0);
                hora.ForeColor = Color.White;
                hora.BackColor = Color.Black;//cor do back ground
                hora.FlatStyle = FlatStyle.Flat;
                hora.BorderStyle = BorderStyle.None;
                hora.Visible = true;
                hora.Parent = relogioForm;
                //criando relogio


                //TEMPO DO RELÓGIO 
                tempo = new System.Timers.Timer();
                tempo.Interval = 1000;
                tempo.Elapsed += CodigosVanda.relogio;
                tempo.AutoReset = true;
                tempo.Start();

                mostrouHora = true;
                
            //hora.ForeColor = Color.FromArgb(fadeCount++, Color.White);

                reconhecedor.SpeakAsync(DateTime.Now.ToString("H:m:s"));
                //fade out label
               


            }//cria um relogio   

        public static void offClock()
        {
            relogioForm.Visible = false;
        }//desativa o relogio

        public static void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
               Opacity += 0.03;
                if (Opacity == 1)
                {
                    timer1.Stop();
                }
        }

        private void button1_Click(object sender, EventArgs e)//iniciar todas as funções da Vanda
        {
            CodigosVanda.tela1();
            CodigosVanda.tempo.Enabled = true;
            timer2.Enabled = true;
            this.Hide();
            i = 0;
            //iniciar = true;//inicia as outras funcionadades da Vanda
            //apresentação
            //reconhecedor.SpeakAsync("Fui criadada para te ajudar de uma forma mais prática, rápida e operacional ");
            //CodigosVanda.legenda.Text = legendaText2.ToString();
            reconhecedor.Volume = 100;
            reconhecedor.Rate = 2;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            mensagem1();
            if (i == 1 && OptionsSelect == false)
            {
                legendaText = "Olá, eu sou a Vanda, " +
                      "é um prazer te conhecer!";

                reconhecedor.SpeakAsync("Olá, eu sou a Vanda, " +
                                        "é um prazer te conhecer!");
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;

            }
            else if (i == 5 && OptionsSelect == false)
            {
                reconhecedor.SpeakAsync("Tudo bêim?");
                legendaText = "Tudo bem?";
   
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
                CodigosVanda.legenda.Location = new Point(160, 310);
            }
            else if(i == 7 && OptionsSelect == false)
            {
                reconhecedor.SpeakAsync("Fui criada para atender todas as necessidades");
                legendaText = "Fui criada para atender todas as necessidades";   
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
                CodigosVanda.legenda.Location = new Point(27, 310);
            }
            else if (i == 11 && OptionsSelect == false)
            {
                reconhecedor.SpeakAsync("Vamos começar? Escolha um modo de operação!");
                legendaText = "Vamos começar? Escolha um modo de operação!";
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
                CodigosVanda.legenda.Location = new Point(50, 310);

            }else if (i == 15)
            {
                timer2.Stop();
                legendaText = "";
                modoAdm = new Button();
                modoAdm.Parent = CodigosVanda.faceVandaMacro;
                modoAdm.Visible = true;
                modoAdm.Location = new Point(160, 350);
                modoAdm.BackColor = Color.DodgerBlue;
                modoAdm.ForeColor = Color.White;
                modoAdm.FlatStyle = FlatStyle.Flat;
                modoAdm.FlatAppearance.BorderSize = 1;
                modoAdm.FlatAppearance.BorderColor = Color.Cyan;
                modoAdm.Text = "Administrativo";
                modoAdm.Size = new Size(100, 25);
                modoAdm.BringToFront();
                modoAdm.Click += new System.EventHandler(ativarModoAdm);

                notSelect = new Button();
                notSelect.Parent = CodigosVanda.faceVandaMacro;
                notSelect.Visible = true;
                notSelect.Location = new Point(160, 300);
                notSelect.BackColor = Color.DodgerBlue;
                notSelect.ForeColor = Color.White;
                notSelect.FlatStyle = FlatStyle.Flat;
                notSelect.FlatAppearance.BorderSize = 1;
                notSelect.FlatAppearance.BorderColor = Color.Cyan;
                notSelect.Text = "Nenhum";
                notSelect.Size = new Size(100, 25);
                notSelect.BringToFront();
                notSelect.Click += new System.EventHandler(dontOptionsSelect);

            }
            //caso nenhum modo seja selecionado, ela se despede
            else if (i == 16 && OptionsSelect == false)//o optionSelect impede que as mensagens de despedida sejam exibida depois que selecionada alguma opção do modo de operação
            {
                reconhecedor.SpeakAsync("Se precisar é só chamar!");
                legendaText = "Se precisar é só chamar!";
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
                CodigosVanda.legenda.Location = new Point(110, 310);
            }
            else if (i == 18 && OptionsSelect == false)//se optionSelect for igual a false, o programa irá exibir essas mensagens ao lado
            {
                reconhecedor.SpeakAsync("Até logo!");
                legendaText = "Até logo!";
                reconhecedor.Volume = 100;
                reconhecedor.Rate = 4;
                CodigosVanda.legenda.Location = new Point(170, 310);
            }
            else if (i == 21 && OptionsSelect == false)
            {      
                legendaText = "";
                timer2.Stop();
            }

            //instruções do modo administrativo ( CASO SEJA SELECIONADO )
            if(i == 16 && admMode == true)
            {
                reconhecedor.SpeakAsync("Tudo bem, vamos trabalhar no modo adiministratívo");
            }

            if(i == 19 && admMode == true)
            {
                reconhecedor.SpeakAsync("Posso enviar dados para o seu banco de dados");
                timer2.Stop();
            }
            // i += 1;
            CodigosVanda.legenda.Text = legendaText.ToString();
        }//apresentações da vanda

        private void label1_Click_1(object sender, EventArgs e)
        {
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(var process in Process.GetProcessesByName("VandaAssistant"))
            {
                process.Kill();
            }
        }//sair do programa

        public void mensagem1()//contador do temporizador de mensagens
        {
            i += 1;
        }

        private void timer3_Tick(object sender, EventArgs e)//este outro tempo é chamado pelo verificador de memória para permitir o acesso as condições de interação ao mostrar o valor da memória
        {//caso contrário, a verificação é feita, mas não haverá componente de texto acessível para mostrar o valor na interface, e este tempo abre uma porta para acessar o Text
            legendaText = Math.Round(result3).ToString() + "%".ToString();//e da um dignóstico

        }
   
        public static void priorityGames()
        {
            foreach(var processos in Process.GetProcesses())
            {
                double memory = processos.PrivateMemorySize64 / 1024 / 1024;
                if (memory > 2.000f)
                {
                    processos.PriorityBoostEnabled = true;
                    reconhecedor.Speak("Desempenho máximo ativado e aplicado para" + '"'+processos.ProcessName+'"');
                    break;
                }
                else
                {
                    reconhecedor.Speak("Não há nenhum processo em consumo excessivo de memória");
                    break;
                }
            }
        }

        public void ativarModoAdm(object sender, EventArgs e)
        {
            OptionsSelect = true;
            admMode = true;
            notSelect.Visible = false;
            modoAdm.Visible = false;//botão do modo administrador
            timer2.Start();
        }

        public void dontOptionsSelect(object sender, EventArgs e)
        {
            OptionsSelect = false;
            notSelect.Visible = false;
            modoAdm.Visible = false;//botão do modo administrador
            timer2.Start();
        }
    }
}
