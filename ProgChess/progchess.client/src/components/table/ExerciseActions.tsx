import { Pencil, Trash } from "lucide-react";

import { DeleteDialog } from "../dialog/DeleteDialog";
import { useState } from "react";
import { Link } from "react-router";

interface ExerciseActionProps {
  id: number;
  deleteFn: () => void;
}

export default function ExerciseAction({ id, deleteFn }: ExerciseActionProps) {
  const [openDialog, setOpenDialog] = useState(false);

  return (
    <>
      <div className="flex items-center space-x-2">
        <Link to={`/admin/exercise/edit/${id}`}>
          <Pencil className="w-5 h-5 text-green-500" />
        </Link>

        <button
          type="button"
          className="cursor-pointer"
          onClick={(e) => {
            e.stopPropagation();
            setOpenDialog(true);
          }}
        >
          <Trash className="w-5 h-5 text-red-500" />
        </button>
      </div>
      <DeleteDialog
        open={openDialog}
        message="Cette action est irréversible. Cela supprimera définitivement l'exercice ainsi que vos résultats associés. Êtes-vous sur de vouloir continuer ?"
        onOpenChange={setOpenDialog}
        deleteFn={deleteFn}
      />
    </>
  );
}
