DIR=~/git/
if [ -d $DIR ]; then
  # Take action if $DIR exists. #
  echo "git folder exists"
else
  ###  Control will jump here if $DIR does NOT exists ###
  echo "Folder  ${DIR} not found. Creating the same."
  #mkdir ~/git/
fi

cd ~/git
DIR=~/git/EventoWriteAPI/
if [ -d $DIR ]; then
  # Take action if $DIR exists. #
  echo "Cleaning up ~/git/EventoWriteAPI folder before cloning."
  rm -r -f ~/git/EventoWriteAPI/
else
  ###  Control will jump here if $DIR does NOT exists ###
  echo "${DIR} not found."
  
fi

echo "Cloning https://github.com/redtopdev/EventoWriteAPI.git"
git clone https://github.com/redtopdev/EventoWriteAPI.git
echo "Building ~/git/EventoWriteAPI/Evento.sln"
dotnet build --configuration debug ~/git/EventoWriteAPI/Evento.sln
echo "Publishing ~/git/EventoWriteAPI/API/Evento.csproj to ~/engaze/api/evento/  folder."
dotnet publish --output ~/engaze/api/evento/ ~/git/EventoWriteAPI/API/Evento.csproj
