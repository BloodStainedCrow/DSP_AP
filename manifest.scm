(use-modules (guix channels)
             (guix inferior)
             (guix profiles)
             (guix ui)
             (srfi srfi-11))

(define channels
  (list (channel
         (name 'guix)
         (url "https://git.guix.gnu.org/guix.git")
         (branch "master")
         (commit
          "71ffb948b32b361d02ec0781e25f1e8f8f5aea75")
         (introduction
          (make-channel-introduction
           "9edb3f66fd807b096b48283debdcddccfea34bad"
           (openpgp-fingerprint
            "BBB0 2DDF 2CEA F6A8 0D1D  E643 A2A0 6DF2 A33A 54FA"))))
        (channel
         (name 'nonguix)
         (url "https://gitlab.com/nonguix/nonguix")
         (branch "master")
         (commit
          "4811a684f4ced58cebfa945c3ccbbca6c26807fd")
         (introduction
          (make-channel-introduction
           "897c1a470da759236cc11798f4e0a5f7d4d59fbc"
           (openpgp-fingerprint
            "2A39 3FFF 68F4 EF7A 3D29  12AF 6F51 20A0 22FB B2D5"))))))

(define inferior
  (inferior-for-channels channels))

(define (pkg spec)
  (let-values (((name version output)
                (package-specification->name+version+output spec)))
    (list (car (lookup-inferior-packages inferior name version))
          output)))
(define (pkgs . args)
  (map pkg args))

(packages->manifest
 (pkgs "bash" "coreutils"
       "mono@6"
       "dotnet@8"))
