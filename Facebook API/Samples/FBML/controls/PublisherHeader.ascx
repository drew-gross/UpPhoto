<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublisherHeader.ascx.cs" Inherits="FBMLSample.controls.PublisherHeader" %>
<style>.box {

  height :70px;
  width : 70px;
  float : left;
  text-align: center;
  border: black 1px;
  margin-right: 10px;
  margin-left: 10px;
  cursor: pointer;
  border: black solid 2px;
  background: orange;
  margin-left: 32px;
  margin-top: 20px;
}
.smiley {
  font-size: 20pt;
  font-weight: bold;
  padding: 0px;
  padding-top: 20px;
}

.box_selected {
  border: 2px dashed black;
  background: #E1E1E1;
}

.title {
 padding-top: 10px;
 font-size: 10px;
 visibility: hidden;
}

.box_selected .title {
  visibility: visible;
}

.box_over .title {
 visibility: visible;
}

</style>
<script>
var cur_picked = -1;
function over(id) {
  document.getElementById("sm_"+id).addClassName("box_over");
}
function out(id) {
  document.getElementById("sm_"+id).removeClassName("box_over");
}

function select(title, mood, id, feed) {
  document.getElementById("sm_"+id).addClassName("box_selected");
  document.getElementById("picked").setValue(id);
  if (feed) {
    Facebook.showFeedDialog("<%= callback %>handlers/feedHandler.aspx", {"picked":id});
  } else {
    Facebook.setPublishStatus(true);
  }
}

function unselect(id) {
  document.getElementById("sm_"+id).removeClassName("box_selected");
}

function picked(i) {
  if (cur_picked!=-1) {
    unselect(cur_picked);
  }
  cur_picked = i;
  select(i);
}
</script>
