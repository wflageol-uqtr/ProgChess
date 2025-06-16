import { z } from "zod";
import AuthCard from "../../components/card/AuthCard";
import AuthLayout from "../../components/layout/AuthLayout";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import Flash from "../../components/flash/Flash";
import { Input } from "../../components/ui/input";
import { Button } from "../../components/ui/button";
import { useState, useTransition } from "react";
import axios from "axios";
import { handleApiError } from "../../utils/apiErrorHandler";

const formSchema = z.object({
  email: z.string().email({ message: "Le courriel est invalide" }),
});

export default function ForgotPassword() {
  const [error, setError] = useState("");
  const [isPending, startTransition] = useTransition();
  const [emailSent, setEmailSent] = useState(false);
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
    },
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    startTransition(async () => {
      try {
        console.log(values);

        await axios.post(
          "http://localhost:5290/api/auth/forgot-password",
          values
        );
        setEmailSent(true);
      } catch (error) {
        handleApiError(error, setError);
        form.reset();
      }
    });
  }
  return (
    <AuthLayout>
      <AuthCard>
        {!emailSent ? (
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
              <Flash type={"error"} message={error} />
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Courriel</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="joedoe@email.com"
                        className="text-white"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />

              <Button
                type="submit"
                className="w-full bg-green-500 cursor-pointer hover:bg-green-600"
                disabled={isPending}
              >
                {isPending ? "Connexion..." : "Connexion"}
              </Button>
            </form>
          </Form>
        ) : (
          <div className="text-center space-y-2">
            <h3 className="text-lg font-semibold text-zinc-100">
              Courriel envoyé à l'adresse fournie.
            </h3>
            <p className="text-sm text-zinc-400">
              Veuillez vérifier votre boîte de réception.
            </p>
          </div>
        )}
      </AuthCard>
    </AuthLayout>
  );
}
