import os

import numpy as np
import json
import Node
import Parsing
class AnalyseFile():
    def __init__(self, path):
        self.path=path

    def analyse(self):
        with open(self.path,'r') as file:
            text=file.read()
            parse = Parsing.Parsing(text)
            parse.removeComAndTrim()
            #print(parse.text)
            val=parse.getImportAndRemove()
            nodes=Node.Node.recursiveAnalyse(parse.text)
            print(nodes[0].strRecursive())
            #with open("resultTest.cs",'w') as w:
             #   w.write(parse.text)
    @staticmethod
    def analyseAuto(pathConfigPath, pathSaveResult):
        brutConfig=""
        with open(pathConfigPath, 'r') as fconfig:
            brutConfig=fconfig.read()
        config=json.loads(brutConfig)
        pathConfigPath=os.path.dirname(pathConfigPath)
        library=[]
        result="{"
        for pathFile,methodes in config["files"].items():
            brutCode=""
            with open(os.path.join(pathConfigPath,pathFile),'r') as file:
                brutCode=file.read()

            parse = Parsing.Parsing(brutCode)
            parse.removeComAndTrim()
            for e in parse.getImportAndRemove():
                if not e in library:
                    library.append(e)
            nodes = Node.Node.recursiveAnalyse(parse.text)
            if len(methodes)==0:
                for (path,e) in Node.Node.getAllNodes(nodes):
                    result+='"'+pathFile+path+'":'+e.getJson()+","
            else:
                raise Exception("Non Implementé (select methode analysé)")
        if len(result)!=1:
            result=result[:-1]
        result+="}"
        lib="["
        for e in library:
            lib+='"'+e+'",'
        if len(lib)!=1:
            lib=lib[:-1]
        lib+=']'
        brutResult='{"imports":'+lib+',"result":'+result+'}'
        with open(pathSaveResult, 'w') as file:
            file.write(brutResult)