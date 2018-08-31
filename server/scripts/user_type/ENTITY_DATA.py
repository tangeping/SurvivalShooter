# -*- coding: utf-8 -*-
import KBEngine
import CmdType
from KBEDebug import *


class TEntityFrame(list):
    """
    """
    def __init__(self):
        """
        """
        list.__init__(self)

    def asDict(self):
        data = {
            "entityid"  : self[0],
            "position"  : self[1],
            "direction" : self[2],
        }
        return data

    def createFromDict(self, dictData):
        self.extend([dictData["entityid"], dictData["position"], dictData["direction"]])
        return self


class ENTITY_DATA_PICKLER:
    def __init__(self):
        pass

    def createObjFromDict(self, dict):
        return TEntityFrame().createFromDict(dict)

    def getDictFromObj(self, obj):
        return obj.asDict()

    def isSameType(self, obj):
        return isinstance(obj, TEntityFrame)


inst = ENTITY_DATA_PICKLER()


