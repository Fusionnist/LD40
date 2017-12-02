<?xml version="1.0" encoding="UTF-8"?>
<tileset name="tiletest" tilewidth="32" tileheight="32" tilecount="16" columns="4">
 <image source="tiletest.png" width="128" height="128"/>
 <tile id="0">
  <objectgroup draworder="index">
   <properties>
    <property name="solid" type="bool" value="true"/>
   </properties>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index">
   <properties>
    <property name="door" type="bool" value="true"/>
    <property name="pool" type="int" value="1"/>
    <property name="solid" type="bool" value="true"/>
   </properties>
  </objectgroup>
 </tile>
 <tile id="3">
  <objectgroup draworder="index">
   <properties>
    <property name="solid" type="bool" value="false"/>
   </properties>
  </objectgroup>
 </tile>
 <tile id="10">
  <objectgroup draworder="index">
   <properties>
    <property name="activate" type="int" value="1"/>
    <property name="debuff" value="doublejump"/>
    <property name="treasure" type="bool" value="true"/>
   </properties>
  </objectgroup>
 </tile>
 <tile id="13">
  <objectgroup draworder="index">
   <properties>
    <property name="hurty" type="bool" value="true"/>
   </properties>
  </objectgroup>
 </tile>
 <tile id="14">
  <objectgroup draworder="index">
   <properties>
    <property name="slippery" type="bool" value="true"/>
    <property name="solid" type="bool" value="true"/>
   </properties>
  </objectgroup>
 </tile>
</tileset>
