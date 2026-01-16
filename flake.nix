{
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
  };

  outputs = { self, nixpkgs, fenix }: let
    pkgs = nixpkgs.legacyPackages."x86_64-linux";
    fenixLib = fenix.packages."x86_64-linux";

    neededPackages = with pkgs; [
      dotnetCorePackages.sdk_9_0-bin
    ];
  in {
    devShells."x86_64-linux".codium = pkgs.mkShell {
      buildInputs = with pkgs; [
        bashInteractive

        (vscode-with-extensions.override {
          # vscode = vscodium;
          vscodeExtensions = with vscode-extensions; [
            ms-dotnettools.csdevkit
            ms-dotnettools.csharp
            ms-dotnettools.vscode-dotnet-runtime
          ];
        })
      ] ++ neededPackages;
      LD_LIBRARY_PATH="$LD_LIBRARY_PATH:${builtins.toString (pkgs.lib.makeLibraryPath neededPackages)}";

      shellHook = ''
        export SHELL="${pkgs.bashInteractive}/bin/bash"
      '';
      
    };
  };
}