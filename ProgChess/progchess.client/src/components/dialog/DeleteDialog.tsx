import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "../../components/ui/dialog";
import { Button } from "../ui/button";

interface DeleteDialogProps {
  open: boolean;
  message: string;
  onOpenChange: (open: boolean) => void;
  deleteFn: () => void;
}

export function DeleteDialog({
  open,
  message,
  onOpenChange,
  deleteFn,
}: DeleteDialogProps) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="bg-zinc-950 ">
        <DialogHeader>
          <DialogTitle className="text-white">Êtes-vous sûr ?</DialogTitle>
          <DialogDescription className="text-gray-200">
            {message}
          </DialogDescription>
        </DialogHeader>
        <DialogFooter>
          <DialogClose asChild>
            <Button className="cursor-pointer">Annuler</Button>
          </DialogClose>{" "}
          <Button
            className="cursor-pointer bg-red-600 hover:bg-red-700"
            onClick={() => deleteFn()}
          >
            Confirmer
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
