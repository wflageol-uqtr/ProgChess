import type { ColumnDef } from "@tanstack/react-table";
import ExerciceAction from "./ExerciseActions";
import api, { clientUrl } from "../../utils/api";
import { toast } from "sonner";
import { useNavigate } from "react-router";
import type { Exercise } from "../../utils/type";
import { Checkbox } from "../ui/checkbox";

export const ExerciseColumns: ColumnDef<Exercise>[] = [
  {
    id: "select",
    header: ({ table }) => (
      <Checkbox
        checked={
          table.getIsAllPageRowsSelected() ||
          (table.getIsSomePageRowsSelected() && "indeterminate")
        }
        onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
        aria-label="Select all"
      />
    ),
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        aria-label="Select row"
      />
    ),
  },
  {
    accessorKey: "id",
    header: "Id",
  },
  {
    accessorKey: "situation",
    header: "Situation",
    cell: ({ row }) => {
      const situation = row.getValue("situation");

      let formatted = "";

      if (typeof situation === "string") {
        formatted =
          situation.length > 20
            ? situation.substring(0, 23) + "..."
            : situation;
      } else {
        formatted = String(situation ?? "");
      }
      return <div className="font-medium">{formatted}</div>;
    },
  },
  {
    accessorKey: "link",
    header: "Lien",
    cell: ({ row }) => {
      const id = row.getValue("id");
      let link = "";

      link = `${clientUrl}/exercise/${id}`;
      const copyToClipboard = () => {
        navigator.clipboard.writeText(link);
        toast.info("Élément copié");
      };
      return (
        <div className="font-medium cursor-pointer" onClick={copyToClipboard}>
          {link}
        </div>
      );
    },
  },
  {
    accessorKey: "unitTests",
    header: "Nombre de tests",
    cell: ({ row }) => {
      const unitTest = row.getValue<[]>("unitTests");
      return <div>{unitTest.length}</div>;
    },
  },
  {
    header: "Action",
    id: "actions",
    cell: ({ row }) => {
      const navigate = useNavigate();
      const handleDelete = async () => {
        try {
          const response = await api.delete(
            `/api/exercise/delete/${row.getValue("id")}`
          );
          toast.success(response.data, {
            className: "bg-green-100",
          });
          navigate(0);
        } catch (error) {
          toast.error("Une erreur est survenue.");
        }
      };

      return <ExerciceAction id={row.getValue("id")} deleteFn={handleDelete} />;
    },
  },
];
