using System;
using System.Collections;
using System.Threading;
namespace ExecutionEngine.Common.Connect
{

    using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
    using Application.Utility.Logging;
    using System.Text.RegularExpressions;
    using System.Linq;

    public abstract class Connect : SessionObjectInterface, TimerEventListener
    {
        private ILogger _logger = new CrucialLogger();
        internal const int DEFAULT_EXPECT_TIMEOUT = 10;
        internal const int DEFAULT_PROCESS_TIMEOUT = -1;
        protected internal string id;
        internal int options = 0;
        internal string content = "";
        internal string matchContent = "";
        internal string matchPattern = null;
        internal string postMatch = null;
        internal sbyte[] matchBytes = new sbyte[0];
        internal sbyte[] postBytes = new sbyte[0];
        internal long expectTimeout = 10L;
        protected internal bool isClosed = true;
        internal volatile bool continueReading = true;
        protected internal ConnectReader reader;
        public virtual void connect()
        {
            this.isClosed = false;
            this.id = Guid.NewGuid().ToString();
        }
        public virtual void close()
        {
            this.isClosed = true;
        }
        ~Connect()
        {
            close();
        }
        public virtual void send()
        {
            if ((this.isClosed) || (this.reader == null))
            {
                throw new Exception("WARN: Connection is not established");
            }
            clear();
        }
        public virtual string read()
        {
            if ((this.isClosed) || (this.reader == null))
            {
                throw new Exception("WARN: Connection is not established");
            }
            return this.reader.read();
        }
        public virtual sbyte[] readByte()
        {
            if ((this.isClosed) || (this.reader == null))
            {
                throw new Exception("WARN: Connection is not established");
            }
            return this.reader.readByte();
        }
        public virtual void clear()
        {
            this.reader.clear();
            this.matchContent = "";
            this.matchBytes = new sbyte[0];
        }
        public virtual string expect()
        {
            return read();
        }
        public virtual string expect(string pattern)
        {
            return expect(pattern, this.expectTimeout);
        }
        public virtual string expect(string pattern, long timeout)
        {
            ArrayList patterns = new ArrayList();
            patterns.Add(pattern);
            return expect(patterns, timeout);
        }
        public virtual string expect(IList patterns)
        {
            return expect(patterns, this.expectTimeout);
        }
        public virtual string expect(IList patterns, long timeout)
        {
            string result = "";
            if ((this.isClosed) || (this.reader == null))
            {
                string msg = "WARN: Connection is not established. ";
                if (this.reader != null)
                {
                    msg = msg + this.reader.read();
                }
                throw new Exception(msg);
            }
            try
            {
                bool found = false;
                Timer tm = null;
                if (timeout != -1L)
                {
                    tm = new Timer(timeout, this);
                    tm.startTimer();
                    this.continueReading = true;
                }
                IEnumerator i;
                for (i = patterns.GetEnumerator(); (i.MoveNext()) && (!found);)
                {
                    string regex = (string)i.Current;
                    Regex regexPattern = new Regex(regex);
                    Match matcher = regexPattern.Match(this.matchContent);
                    //   Pattern pattern = Pattern.compile(regex, this.options);
                    //   Matcher matcher = pattern.matcher(this.matchContent);
                    this.matchPattern = regex;
                    if (matcher.Success)
                    {
                        found = true;
                        this.matchContent = StringHelperClass.SubstringSpecial(this.matchContent, matcher.Index, this.matchContent.Length);
                    }
                }
                while ((this.continueReading) && (!found))
                {
                    if (!this.reader.hasContent())
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        string readContent = this.reader.read();
                        this.matchContent += readContent;
                        this.content += readContent;
                        _logger.Trace("matchContent: " + this.matchContent);
                        result = this.matchContent;
                        for (i = patterns.GetEnumerator(); (i.MoveNext()) && (!found);)
                        {
                            string regex = (string)i.Current;
                            Regex regexPattern = new Regex(regex);
                            Match matcher = regexPattern.Match(this.matchContent);
                            //   Pattern pattern = Pattern.compile(regex, this.options);
                            // Matcher matcher = pattern.matcher(this.matchContent);
                            this.matchPattern = regex;
                            if (matcher.Success)
                            {
                                found = true;
                                this.matchContent = StringHelperClass.SubstringSpecial(this.matchContent, matcher.Index, this.matchContent.Length);
                                this.postMatch = this.matchContent;
                                _logger.Trace("pattern: " + regexPattern);
                                _logger.Trace("postContent: " + this.postMatch);
                            }
                        }
                    }
                }
                if (tm != null)
                {
                    tm.Status = 4;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Connector expect exception: " + e.Message);
            }
            return result;
        }
        public virtual string expectByte()
        {
            return read();
        }
        public virtual sbyte[] expect(sbyte[] pattern)
        {
            return expect(pattern, this.expectTimeout);
        }
        public virtual sbyte[] expect(sbyte[] pattern, long timeout)
        {
            sbyte[] result = null;
            if ((this.isClosed) || (this.reader == null))
            {
                string msg = "WARN: Connection is not established. ";
                throw new Exception(msg);
            }
            try
            {
                bool found = false;
                Timer tm = null;
                if (timeout != -1L)
                {
                    tm = new Timer(timeout, this);
                    tm.startTimer();
                    this.continueReading = true;
                }
                while ((this.continueReading) && (!found))
                {
                    if (!this.reader.hasContent())
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        sbyte[] readContent = this.reader.readByte();
                        //  this.matchBytes = Bytes.concat(new sbyte[][] { this.matchBytes, readContent });
                        result = this.matchBytes.Concat(readContent).ToArray();
                        int location = SearchBytes(this.matchBytes, pattern);
                        int patternLen = pattern.Length;
                        if (location >= 0)
                        {
                            found = true;
                            sbyte[] temp = this.matchBytes;
                            this.matchBytes = new sbyte[0];
                            Array.Copy(temp, location + patternLen + 1, this.matchBytes, 0, this.matchBytes.Length - 1);
                            //   this.matchBytes = Array.Copy(this.matchBytes, location + patternLen + 1, this.matchBytes.Length - 1);
                            Array.Copy(this.matchBytes, this.postBytes, this.matchBytes.Length);
                            _logger.Trace("Total bytes found : " + this.matchBytes.Length);
                            _logger.Trace("postBytes length: " + this.postBytes.Length);
                        }
                    }
                }

                if (tm != null)
                {
                    tm.Status = 4;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Connector expect exception: " + e.Message);
            }
            return result;
        }

        static int SearchBytes(sbyte[] haystack, sbyte[] needle)
        {
            var len = needle.Length;
            var limit = haystack.Length - len;
            for (var i = 0; i <= limit; i++)
            {
                var k = 0;
                for (; k < len; k++)
                {
                    if (needle[k] != haystack[i + k]) break;
                }
                if (k == len) return i;
            }
            return -1;
        }
        public virtual void timerTimedOut()
        {
            lock (this)
            {
                this.continueReading = false;
            }
        }
        public virtual void timerInterrupted(ThreadInterruptedException ioe)
        {
            this.continueReading = false;
        }
        public virtual bool LastExpectTimeout
        {
            get
            {
                return !this.continueReading;
            }
        }
        public virtual string Content
        {
            get
            {
                return this.content;
            }
        }
        public virtual string PostMatch
        {
            get
            {
                return this.postMatch;
            }
        }
        public virtual string MatchPattern
        {
            get
            {
                return this.matchPattern;
            }
        }
        public virtual int Options
        {
            get
            {
                return this.options;
            }
            set
            {
                this.options = value;
            }
        }
        public virtual void sleep(int secs)
        {
            try
            {
                Thread.Sleep(secs * 1000);
            }
            catch (Exception)
            {
            }
        }
        public virtual long DefaultTimeout
        {
            set
            {
                this.expectTimeout = value;
            }
        }
        public virtual bool Closed
        {
            get
            {
                return this.isClosed;
            }
            set
            {
                this.isClosed = value;
            }
        }
        public virtual string Id
        {
            get
            {
                if (string.ReferenceEquals(this.id, null))
                {
                    this.id = Guid.NewGuid().ToString();
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = 31 * result + (string.ReferenceEquals(this.id, null) ? 0 : this.id.GetHashCode());
            return result;
        }
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Connect))
            {
                return false;
            }
            Connect other = (Connect)obj;
            if (string.ReferenceEquals(this.id, null))
            {
                if (!string.ReferenceEquals(other.id, null))
                {
                    return false;
                }
            }
            else if (!this.id.Equals(other.id))
            {
                return false;
            }
            return true;
        }
    }
}