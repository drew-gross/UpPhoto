<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
<title>UpPhoto - About.</title>
<link type="text/css" rel="stylesheet" href="styles.css" />
</head>
<body>
<div id="Background">
<div id="Main">Up-Photo
<div id="Info">
<?php
if (!isset($_POST["comments"]))
{
echo "<p>
<form action=\"contact.php\" method=\"post\">
Type your comments: <input type=\"text\" name=\"comments\" />
<input type=\"submit\" value=\"Send your comments\">
</form>
</p>";
}
else 
{
$comments = fopen("comments.txt", "a");
fwrite($comments, $_POST["comments"]);
echo "<p>Thank you for your comments.</p>";
}
?>
</div>
</div>
<?php
require("footer.php");
?></div>
</body>
</html>
