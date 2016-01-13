path "%ProgramFiles(x86)%\MSBuild\12.0\Bin

csc.exe /target:library /recurse:*.cs /reference:GeometryFriendsFiles\x86\Debug\GeometryFriends.exe;"%ProgramFiles(x86)%\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.Game.dll";"%ProgramFiles(x86)%\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.dll"
