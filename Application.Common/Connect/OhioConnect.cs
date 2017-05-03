using System;
using System.Threading;
namespace ExecutionEngine.Common.Connect
{
    using OhioManager = com.icominfo.Ohio.OhioManager;
    using OhioPosition = com.icominfo.Ohio.OhioPosition;
    using OhioScreen = com.icominfo.Ohio.OhioScreen;
    using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
    using Log = com.resolve.util.Log;
    using Application.Utility.Logging;
    using iOhioOIA = org.ohio.iOhioOIA;
    using iOhioOIAListener = org.ohio.iOhioOIAListener;
    using iOhioPosition = org.ohio.iOhioPosition;
    using iOhioScreen = org.ohio.iOhioScreen;
    using iOhioScreenListener = org.ohio.iOhioScreenListener;
    using iOhioSession = org.ohio.iOhioSession;
    using iOhioSessionListener = org.ohio.iOhioSessionListener;
    public class OhioConnect : iOhioOIAListener, iOhioScreenListener, iOhioSessionListener, SessionObjectInterface
    {
        private ILogger _logger = new CrucialLogger();
        internal static OhioManager manager = new OhioManager();
        internal int emulation;
        internal string host;
        internal int port;
        internal string sessionName;
        internal iOhioSession session;
        internal int m_InputInhibited = -1;
        internal int m_Owner = -1;
        internal int m_screen = -1;
        public OhioConnect(string emul, string host, int port)
        {
            int emulation;
            if (emul.Equals("TN3270", StringComparison.CurrentCultureIgnoreCase))
            {
                emulation = 134;
            }
            else
            {
                if (emul.Equals("TN5250", StringComparison.CurrentCultureIgnoreCase))
                {
                    emulation = 138;
                }
                else
                {
                    throw new Exception("Invalid emulation type: " + emul);
                }
            }
            this.emulation = emulation;
            this.host = host;
            this.port = port;
            this.sessionName = ("" + DateTimeHelperClass.CurrentUnixTimeMillis());
        }
        public virtual void connect()
        {
            if ((this.session == null) || (!this.session.Connected))
            {
                manager.ControllerClassName = "com.icominfo.Controller.Oem.Oem";
                string connectionString = "Emul=" + this.emulation + "&Param=[Com1]Service.Adresse TCP_IP=" + this.host + ";Service.Port=" + this.port + ";";
                this.session = manager.openSession(connectionString, this.sessionName);
                this.session.Screen.addScreenListener(this);
                this.session.Screen.OIA.addOIAListener(this);
                this.session.addSessionListener(this);
                this.session.connect();
            }
        }
        public virtual void disconnect()
        {
            if (this.session != null)
            {
                this.session.disconnect();
            }
            if (manager != null)
            {
                manager.closeSession(this.session);
            }
        }
        public virtual void close()
        {
            try
            {
                disconnect();
            }
            catch (Exception e)
            {
                _logger.Warn("Error closing connection: " + e.Message);
            }
        }
        ~OhioConnect()
        {
            try
            {
                disconnect();
            }
            catch (Exception e)
            {
                _logger.Warn("Error closing connection: " + e.Message);
            }
        }
        public virtual void onSessionChanged(int inUpdate)
        {
            if (inUpdate == 1)
            {
                _logger.Debug("Session connected");
            }
            if (inUpdate == 0)
            {
                _logger.Debug("Session disconnected");
            }
        }
        public virtual void onScreenChanged(int inUpdate, iOhioPosition inStart, iOhioPosition inEnd)
        {
            if (inUpdate == 1)
            {
                _logger.Trace("Screen updated by client");
            }
            if (inUpdate == 0)
            {
                _logger.Trace("Screen updated by host");
                this.m_screen = inUpdate;
            }
        }
        public virtual void onOIAChanged()
        {
            iOhioOIA oia = this.session.Screen.OIA;
            this.m_InputInhibited = oia.InputInhibited;
            switch (this.m_InputInhibited)
            {
                case 2:
                    _logger.Trace("InputInhibited=COMMCHECK");
                    break;
                case 4:
                    _logger.Trace("InputInhibited=MACHINECHECK");
                    break;
                case 0:
                    _logger.Trace("InputInhibited=NOTINHIBITED");
                    break;
                case 5:
                    _logger.Trace("InputInhibited=OTHER");
                    break;
                case 3:
                    _logger.Trace("InputInhibited=PROGCHECK");
                    break;
                case 1:
                    _logger.Trace("InputInhibited=SYSTEM_WAIT");
                    break;
            }
            this.m_Owner = oia.Owner;
            switch (this.m_Owner)
            {
                case 1:
                    _logger.Trace("Owner=APP");
                    /* 232 */
                    break;
                case 2:
                    /* 234 */
                    _logger.Trace("Owner=NVT");
                    /* 235 */
                    break;
                case 4:
                    /* 237 */
                    _logger.Trace("Owner=SSCP");
                    /* 238 */
                    break;
                case 0:
                    /* 240 */
                    _logger.Trace("Owner=UNKNOWN");
                    /* 241 */
                    break;
                case 3:
                    /* 243 */
                    _logger.Trace("Owner=UNOWNED");
                    break;
            }
        }
        public virtual iOhioPosition Cursor
        {
            get
            {       /* 253 */
                return this.session.Screen.Cursor;
            }
            set
            {       /* 258 */
                this.session.Screen.Cursor = value;
            }
        }
        public virtual void setCursor(int row, int col)
        {   /* 263 */
            this.session.Screen.Cursor = new OhioPosition(row, col);
        }
        public virtual OhioScreen Screen
        {
            get
            {       /* 271 */
                return (OhioScreen)this.session.Screen;
            }
        }
        public virtual string getData(iOhioPosition position, int len)
        {   /* 280 */
            return new string(((OhioScreen)this.session.Screen).getData(position, len, 1));
        }
        public virtual string getData(int row, int col, int len)
        {   /* 285 */
            return new string(((OhioScreen)this.session.Screen).getData(new OhioPosition(row, col), len, 1));
        }
        public virtual string getData(iOhioPosition startPos, iOhioPosition endPos)
        {   /* 290 */
            return new string(this.session.Screen.getData(startPos, endPos, 1));
        }
        public virtual string getData(int startRow, int startCol, int endRow, int endCol)
        {   /* 295 */
            return new string(((OhioScreen)this.session.Screen).getData(new OhioPosition(startRow, startCol), new OhioPosition(endRow, endCol), 1));
        }
        public virtual void send(string value)
        {   /* 305 */
            this.session.Screen.setString(value, null);
        }
        public virtual void send(string value, iOhioPosition position)
        {   /* 310 */
            this.session.Screen.setString(value, position);
        }
        public virtual void send(string value, int row, int col)
        {   /* 315 */
            this.session.Screen.setString(value, new OhioPosition(row, col));
        }
        public virtual void sendEnter()
        {   /* 323 */
            this.session.Screen.sendKeys("[enter]", null);
        }
        public virtual void sendKey(string value)
        {   /* 328 */
            this.session.Screen.sendKeys(value, null);
        }
        public virtual void sendKey(string value, iOhioPosition position)
        {   /* 333 */
            this.session.Screen.sendKeys(value, position);
        }
        public virtual void sendKey(string value, int row, int col)
        {   /* 338 */
            this.session.Screen.sendKeys(value, new OhioPosition(row, col));
        }
        public virtual void sendKey(int value)
        {   /* 343 */
            ((OhioScreen)this.session.Screen).sendFunctionKey(value, null);
        }
        public virtual void sendKey(int value, OhioPosition position)
        {   /* 348 */
            ((OhioScreen)this.session.Screen).sendFunctionKey(value, position);
        }
        public virtual void sendKey(int value, int row, int col)
        {   /* 353 */
            ((OhioScreen)this.session.Screen).sendFunctionKey(value, new OhioPosition(row, col));
        }
        public virtual void sendFunctionKey(int value)
        {   /* 358 */
            ((OhioScreen)this.session.Screen).sendFunctionKey(value, null);
        }
        public virtual void sendAidKey(int value)
        {   /* 363 */
            this.session.Screen.sendAid(value);
        }
        public virtual bool waitForScreen()
        {   /* 382 */
            return waitForScreen(10);
        }
        public virtual bool waitForScreen(int timeout)
        {   /* 388 */
            bool result = true;
            /* 390 */
            int retry = timeout;
            try
            {   /* 393 */
                while ((retry > 0) && ((this.m_screen != 0) || (this.m_InputInhibited != 0)))
                {   /* 395 */
                    Thread.Sleep(1000);
                    /* 396 */
                    retry--;
                }
            }
            catch (Exception)
            {   /* 401 */
                result = false;
            }
            /* 405 */
            if (retry == 0)
            {   /* 407 */
                result = false;
            }
            /* 410 */
            this.m_screen = -1;
            /* 412 */
            return result;
        }
        public virtual string printScreen()
        {   /* 420 */
            return this.session.Screen.String;
        }
        public static void pause()
        {
            try
            {   /* 508 */
                while (Console.Read() != 13)
                {
                }
            }
            catch (Exception)
            {
            }
        }
        /* 521 */
        public static string OHIO_KEY_ATTN = "attn";
        /* 522 */
        public static string OHIO_KEY_BACKSPACE = "backspace";
        /* 523 */
        public static string OHIO_KEY_BACKTAB = "backtab";
        /* 524 */
        public static string OHIO_KEY_CLEAR = "clear";
        /* 525 */
        public static string OHIO_KEY_CURSORSELECT = "cursorselect";
        /* 526 */
        public static string OHIO_KEY_DELETE = "delete";
        /* 527 */
        public static string OHIO_KEY_DOWN = "down";
        /* 528 */
        public static string OHIO_KEY_DUP = "dup";
        /* 529 */
        public static string OHIO_KEY_ENTER = "enter";
        /* 530 */
        public static string OHIO_KEY_ERASEEOF = "eraseeof";
        /* 531 */
        public static string OHIO_KEY_ERASEINPUT = "eraseinput";
        /* 532 */
        public static string OHIO_KEY_FASTDOWN = "fastdown";
        /* 533 */
        public static string OHIO_KEY_FASTLEFT = "fastleft";
        /* 534 */
        public static string OHIO_KEY_FASTRIGHT = "fastright";
        /* 535 */
        public static string OHIO_KEY_FASTUP = "fastup";
        /* 536 */
        public static string OHIO_KEY_FIELDMARK = "fieldmark";
        /* 537 */
        public static string OHIO_KEY_HOME = "home";
        /* 538 */
        public static string OHIO_KEY_INSERT = "insert";
        /* 539 */
        public static string OHIO_KEY_LEFT = "left";
        /* 540 */
        public static string OHIO_KEY_NEWLINE = "newline";
        /* 541 */
        public static string OHIO_KEY_PA1 = "pa1";
        /* 542 */
        public static string OHIO_KEY_PA2 = "pa2";
        /* 543 */
        public static string OHIO_KEY_PA3 = "pa3";
        /* 544 */
        public static string OHIO_KEY_PF1 = "pf1";
        /* 545 */
        public static string OHIO_KEY_PF10 = "pf10";
        /* 546 */
        public static string OHIO_KEY_PF11 = "pf11";
        /* 547 */
        public static string OHIO_KEY_PF12 = "pf12";
        /* 548 */
        public static string OHIO_KEY_PF13 = "pf13";
        /* 549 */
        public static string OHIO_KEY_PF14 = "pf14";
        /* 550 */
        public static string OHIO_KEY_PF15 = "pf15";
        /* 551 */
        public static string OHIO_KEY_PF16 = "pf16";
        /* 552 */
        public static string OHIO_KEY_PF17 = "pf17";
        /* 553 */
        public static string OHIO_KEY_PF18 = "pf18";
        /* 554 */
        public static string OHIO_KEY_PF19 = "pf19";
        /* 555 */
        public static string OHIO_KEY_PF2 = "pf2";
        /* 556 */
        public static string OHIO_KEY_PF20 = "pf20";
        /* 557 */
        public static string OHIO_KEY_PF21 = "pf21";
        /* 558 */
        public static string OHIO_KEY_PF22 = "pf22";
        /* 559 */
        public static string OHIO_KEY_PF23 = "pf23";
        /* 560 */
        public static string OHIO_KEY_PF24 = "pf24";
        /* 561 */
        public static string OHIO_KEY_PF3 = "pf3";
        /* 562 */
        public static string OHIO_KEY_PF4 = "pf4";
        /* 563 */
        public static string OHIO_KEY_PF5 = "pf5";
        /* 564 */
        public static string OHIO_KEY_PF6 = "pf6";
        /* 565 */
        public static string OHIO_KEY_PF7 = "pf7";
        /* 566 */
        public static string OHIO_KEY_PF8 = "pf8";
        /* 567 */
        public static string OHIO_KEY_PF9 = "pf9";
        /* 568 */
        public static string OHIO_KEY_RESET = "reset";
        /* 569 */
        public static string OHIO_KEY_RIGHT = "right";
        /* 570 */
        public static string OHIO_KEY_SYSREQ = "sysreq";
        /* 571 */
        public static string OHIO_KEY_TAB = "tab";
        /* 572 */
        public static string OHIO_KEY_UP = "up";
    }
}