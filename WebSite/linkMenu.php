<?php
function navlink($target, $name, $selected){
  if($selected){
    echo "<li><a href=\"$target\" class=\"selected\">$name</a></li>";
  } else {
    echo "<li><a href=\"$target\">$name</a></li>";
  }
}

function navlinks($current){
  $links = array("/" => "Home",
  				 "contact.php" => "Contact Us",
  				 "instructions.php" => "How It Works");
?>
<div>
<div style="float: left">
<a href="/"><img src="UpPhoto_SMALLLOGO_transparent.png" alt="UpPhoto Logo" style="border: none; outline: 0;"/></a>
</div>
<?php 
  echo "<ul class=\"navigation\">";
  foreach($links as $link => $name){
    navlink($link, $name, $name == $current);
  }
  echo "</ul>";
?>
<div style="clear: both"></div>
<?php 
}
?>
</div>