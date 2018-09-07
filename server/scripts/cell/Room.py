# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import GameConfigs
import random
import GameUtils

from ENTITY_DATA import TEntityFrame
from FRAME_DATA import TFrameData
from FRAME_LIST import TFrameList


TIMER_TYPE_ROOM_TICK = 1
TIMER_TYPE_DESTORY = 2

class Room(KBEngine.Entity):
	"""
	游戏场景
	"""
	def __init__(self):
		KBEngine.Entity.__init__(self)

		# 把自己移动到一个不可能触碰陷阱的地方
		self.position = (0.0, 0.0, 0.0)

		self.avatars = {}

		self.frameBegin = False

		# 告诉客户端加载地图
		KBEngine.addSpaceGeometryMapping(self.spaceID, None, "spaces/gameMap")

		DEBUG_MSG('created space[%d] entityID = %i, res = %s.' % (self.roomKeyC, self.id, "spaces/gameMap"))

		# 让baseapp和cellapp都能够方便的访问到这个房间的entityCall
		KBEngine.globalData["Room_%i" % self.spaceID] = self.base

		self.addTimer(1,0.00001,TIMER_TYPE_ROOM_TICK)
		#self.addTimer(600,0,TIMER_TYPE_DESTORY)

#		DEBUG_MSG("Room::__init__,currFrame:%s,len:%i" % (str(self.currFrame),len(self.currFrame)))
	#--------------------------------------------------------------------------------------------
	#                              Callbacks
	#--------------------------------------------------------------------------------------------
	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
#		DEBUG_MSG("Room::onTimer %d " % userArg)
		if userArg == TIMER_TYPE_ROOM_TICK and self.frameBegin:
			self.onBroadFrameBegine()

		elif userArg == TIMER_TYPE_DESTORY:
			self.destroySpace()

	def onDestroy(self):
		"""
		KBEngine method.
		"""
		DEBUG_MSG("Room::onDestroy: %i" % (self.id))
		del KBEngine.globalData["Room_%i" % self.spaceID]


	def onDestroyTimer(self):
		DEBUG_MSG("Room::onDestroyTimer: %i" % (self.id))
		# 请求销毁引擎中创建的真实空间，在空间销毁后，所有该空间上的实体都被销毁
		self.destroySpace()


	def onEnter(self, entityCall):
		"""
		defined method.
		进入场景
		"""
		DEBUG_MSG('Room-cell::onEnter space[%d] entityID = %i.' % (self.spaceID, entityCall.id))
		self.avatars[entityCall.id] = entityCall

	def onLeave(self, entityID):
		"""
		defined method.
		离开场景
		"""
		DEBUG_MSG('Room::onLeave space[%d] entityID = %i.' % (self.spaceID, entityID))
		del self.avatars[entityID]

	def broadMessage(self):

		for e in self.avatars.values():
			if e is None or e.client is None:
				continue
			for frameid in range(e.frameId,self.roomFarmeId):
#				DEBUG_MSG('Room.py 88 line  : %s' % str(self.framePool[frameid+1]))
				e.client.onRspFrameMessage(self.framePool[frameid+1])
				
			e.frameId = self.roomFarmeId


	def addFrame(self,entityCall, framedata):
		"""
		添加数据帧
		"""
		
		if entityCall is None :
			return

#		DEBUG_MSG("Room:: addFrame:%s" % (str(framedata)))

		operation = TEntityFrame().createFromDict({"entityid":framedata[0],"cmd_type":framedata[1],"datas":framedata[2]})

		if len(self.currFrame) <= 0 or len(self.currFrame[1]) <= 0:			
			self.currFrame = TFrameData().createFromDict({"frameid":self.roomFarmeId,"operation":[operation]})
		else:
			self.currFrame[1].append(operation)
		
		self.frameBegin = True


	def onBroadFrameBegine(self):

		if not self.frameBegin:
			return

		self.roomFarmeId += 1

		sendFrame = TFrameData()

#		DEBUG_MSG("Room::onBroadFrameBegin,currFrame:%s,len:%i" % (str(self.currFrame),len(self.currFrame)))

		if len(self.currFrame) <= 0 or len(self.currFrame[1]) <= 0:
			operation = TEntityFrame().createFromDict({"entityid":0,"cmd_type":0,"datas":b''})
			sendFrame = TFrameData().createFromDict({"frameid":self.roomFarmeId,"operation":[operation]})			
		else :
			sendFrame = self.currFrame

#		DEBUG_MSG("--------------------framePool before :%s" % str(self.framePool))
		self.framePool[self.roomFarmeId] = sendFrame
#		DEBUG_MSG("--------------------framePool after :%s" % str(self.framePool))

		self.broadMessage()

#		DEBUG_MSG("Room::onBroadFrameBegin,sendFrame:%s" % str(sendFrame))

		self.onBroadFrameEnd()

	def onBroadFrameEnd(self):
		"""
		define
		"""
#		DEBUG_MSG("Room::1111111111111111")
		self.currFrame.clear()
#		DEBUG_MSG("Room::22222222222222222")




