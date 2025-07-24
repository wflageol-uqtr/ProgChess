import type { ColumnDef } from "@tanstack/react-table";
import { useNavigate } from "react-router";
import api from "../../utils/api";
import { toast } from "sonner";
import { handleApiError } from "../../utils/apiErrorHandler";
import StudentExerciseAction from "./StudentExerciseAction";
import { Checkbox } from "../ui/checkbox";
import type { Exercise, StudentExerciseGroup } from "../../utils/type";

export const StudentExerciseColumns: ColumnDef<StudentExerciseGroup>[] = [
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
    accessorKey: "studentPermanentCode",
    header: "Code permanent",
  },
  {
    accessorKey: "exercises",
    header: "Exercice inscrit",
    cell: ({ row }) => {
      const exercises = row.getValue<[]>("exercises");
      const ids = exercises?.map((e: Exercise) => e.id).join(", ") || "";
      return <span>{ids}</span>;
    },
  },

  {
    accessorKey: "createdAt",
    header: "Date de création",
  },
  {
    accessorKey: "updatedAt",
    header: "Date de modification",
  },
  {
    header: "Actions",
    id: "actions",
    cell: ({ row }) => {
      const navigate = useNavigate();

      const handleDelete = async () => {
        try {
          console.log(row.getValue("studentPermanentCode"));

          const response = await api.delete(
            `/api/studentexercise/delete/${row.getValue(
              "studentPermanentCode"
            )}`
          );
          toast.success(response.data, {
            className: "bg-green-100",
          });
          navigate(0);
        } catch (error) {
          handleApiError(error);
        }
      };
      return (
        <StudentExerciseAction
          id={row.getValue("id")}
          deleteFn={handleDelete}
        />
      );
    },
  },
];
