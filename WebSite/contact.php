<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
	<?php require("headerInfo.php");
	      outputHeaders("Contact Us"); ?>
	<body>
<?php require("linkMenu.php");
      navlinks("Contact Us") ?>
<div class="container_12">
<div class="grid_12">
<?php
if (isset($_REQUEST["comment"]))
{
mail("comments@upphoto.ca", "UpPhoto comment", $_REQUEST["comment"]);
?>
<p>Thank you for your comments!</p>
<?php
}
else 
{
?>
<p>
<div id="Centered">
<p>
Send us a comment about UpPhoto.
</p>
<form action="contact.php" method="post">
<textarea name="comment" rows="15" cols="40">Enter your comments here.</textarea><br />
<input type="submit" value="Send your comments" />
</form>
</div>
</p>
<?php
}
?>
</div>
</div>
</body>
</html>
