<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProfileBox.ascx.cs" Inherits="FBMLSample.controls.ProfileBox" %>
<style> 
            h2 {
              text-align: center;
              font-size: 11px;
              color:#3B5998;
              }

              .smiley {
                font-size: 35pt;
                font-weight: bold;
                padding: 20px;
              }
              .smile {
              padding: 10px;
              width : 100px;
              float : left;
              text-align: center;
              border: black 1px;
              margin-right: 10px;
              margin-left: 10px;
              cursor: pointer;
              border: black solid 2px;
              background: orange;
              margin-left: 32px;
              margin-top: 30px;
              margin-bottom: 20px;
              }

              </style>
              <h2>We are pleased to announce that <fb:name useyou="false" uid="profileowner" /> is feeling:</h2>
              <div class="smile"><div class="smiley">:)</div></div>
              <br />
              <p>
              <%=link %>
              </p>