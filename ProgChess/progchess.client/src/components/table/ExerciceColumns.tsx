import type { ColumnDef } from "@tanstack/react-table";
import ExerciceAction from "./ExerciceActions";
import { api } from "../../utils/api";
import { toast } from "sonner";
import { useNavigate } from "react-router";
import type { Exercice } from "../../utils/type";

export const ExerciceColumns: ColumnDef<Exercice>[] = [
  {
    accessorKey: "id",
    header: "Numéro",
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
            `/api/exercice/delete/${row.getValue("id")}`
          );
          toast(response.data);
          navigate(0);
        } catch (error) {
          toast("Une erreur est survenue.");
        }
      };

      return <ExerciceAction id={row.getValue("id")} deleteFn={handleDelete} />;
    },
  },
];
