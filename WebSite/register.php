<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
	<?php require("headerInfo.php");
	      outputHeaders("UpPhoto for Mac OSX"); ?>
	<body>
<?php require("linkMenu.php");
      navlinks("") ?>
<div>
<div>
<?php
if (isset($_REQUEST["email"]))
{
mail("waitlist@upphoto.ca", "UpPhoto OSX register", $_REQUEST["comment"]);
?>
<p>We'll let you know as soon as it is released!</p>
<?php
}
else 
{
?>
<p>
<div>
<p>
Unfortunately, UpPhoto for Mac OSX is not quite ready. You can give us your email below and we'll let you know when it is released.
</p>
<form action="register.php" method="post">
<input type="text" name="email"></input><br />
<input type="submit" value="Let me know" />
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
