
class Parsing():
    def __init__(self, text):
        self.text=text
    def removeComAndTrim(self):
        res=""
        com=False
        sl=False
        last=''
        cot=False
        dCot=False
        antiCote=False
        self.text+="  "
        for i in range(len(self.text)):
            c=self.text[i]
            if c == '\\':
                if c==last:
                    antiCote=not antiCote
            else:
                antiCote=False
            if c == ' ':
                if last != c and not cot and not dCot:
                    res += c
                elif last==';':
                    c=last
            elif c == ';':
                if not sl and not com:
                    if last == ' ':
                        res=res[:-1]
                    if self.text[i+1]!=c:
                        res+=c
                c=' '
            elif c == '\n':
                sl = False
                c=' '
            elif c == '/':
                if com and last=='*':
                    com=False
                elif not sl and not com:
                    if not cot and not dCot:
                        s=self.text[i+1]
                        if c == s:
                            sl=True
                            c=' '
                            res=res.rstrip()
                        elif s=='*':
                            com=True
                            res = res.rstrip()
                            c=' '
                        else:
                            res+=c
            elif not sl and not com:
                if c == "'":
                    if (antiCote or last != '\\') and not dCot:
                        cot=not cot
                        res += c
                elif c == '"':
                    if (antiCote or last != '\\') and not cot:
                        dCot= not dCot
                        res += c
                else:
                    res+=c
            else:
                c=' '
            last=c
        self.text=res

    def getImportAndRemove(self):
        result=self.text.split("using ")
        self.text=result.pop(0)
        res=[]
        for v in result:
            lib=""
            sep=False
            for c in v:
                if sep:
                    self.text+=c
                elif c==';':
                    sep=True
                else:
                    lib+=c
            res.append(lib)
        return res


