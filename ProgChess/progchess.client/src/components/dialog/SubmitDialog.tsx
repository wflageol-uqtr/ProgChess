import { Button } from "../ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "../ui/dialog";

interface InfoDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  submitFn: () => void;
}

export default function SubmitDialog({
  open,
  onOpenChange,
  submitFn,
}: InfoDialogProps) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="bg-zinc-950 ">
        <DialogHeader>
          <DialogTitle className="text-white">Êtes-vous sûr ?</DialogTitle>
          <DialogDescription className="text-gray-200">
            Une fois le code soumis, il ne sera plus possible de revenir en
            arrière.
          </DialogDescription>
        </DialogHeader>
        <DialogFooter>
          <DialogClose asChild>
            <Button className="cursor-pointer">Annuler</Button>
          </DialogClose>{" "}
          <Button
            className="cursor-pointer bg-green-600 hover:bg-green-700"
            onClick={() => submitFn()}
          >
            Soummettre
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
